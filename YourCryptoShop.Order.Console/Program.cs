﻿using YourCryptoShop.Data;
using YourCryptoShop.Data.Repositories;
using YourCryptoShop.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using YourCryptoShop.Data.Services.RpcServices;

namespace YourCryptoShop.Order.Console
{
    internal class Program
    {
        static async Task Main()
        {
            var _configuration = GetConfig();
            string connectionString = _configuration.GetConnectionString("YourCryptoShop") ?? throw new Exception("connectionString 'DBBCommerce' not set.");
            string rpcDaemonUrl = _configuration.GetSection("RpcSettings:DaemonUrl").Value ?? throw new Exception("RPC setting 'DaemonUrl' not set.");
            string rpcUsername = _configuration.GetSection("RpcSettings:Username").Value ?? throw new Exception("RPC setting 'Username' not set.");
            string rpcPassword = _configuration.GetSection("RpcSettings:Password").Value ?? throw new Exception("RPC setting 'Password' not set.");
            string rpcWalletPassphrase = _configuration.GetSection("RpcSettings:WalletPassphrase").Value ?? throw new Exception("RPC setting 'WalletPassphrase' not set.");

            DataAccessLayer dataAccessLayer = new(connectionString);
            OrderRepository orderRepository = new(dataAccessLayer);
            CryptoWalletRepository cryptoWalletRepository = new(dataAccessLayer);
            RpcService rpcService = new(rpcDaemonUrl, rpcUsername, rpcPassword);
            Shop2CryptoWalletRepository shop2CryptoWalletRepository = new(dataAccessLayer);
            TransactionRepository transactionRepository = new(dataAccessLayer);

            StringBuilder sbLog = new();
            Log($"Start {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Retrieve crypto wallets and construct dictionary
            Dictionary<Guid, CryptoWallet> dictCryptoWallets = [];
            var cryptoWallets = await cryptoWalletRepository.Get(new Domain.Parameters.GetCryptoWalletsParameters());

            foreach (var cryptoWallet in cryptoWallets)
                dictCryptoWallets.Add(cryptoWallet.Id!.Value, cryptoWallet);

            // Retrieve shops and crypto wallets and construct dictionary
            var shopCryptoWallets = await shop2CryptoWalletRepository.Get(new Domain.Parameters.GetShop2CryptoWalletsParameters());
            Dictionary<Guid, Dictionary<Guid, string>> dictCryptoWalletAddressPerCurrencyPerShop = [];

            foreach (var shopCryptoWallet in shopCryptoWallets)
            {
                CryptoWallet? cryptoWallet = null;

                if (dictCryptoWallets.TryGetValue(shopCryptoWallet.CryptoWalletId, out CryptoWallet? valueCryptoWallet))
                    cryptoWallet = valueCryptoWallet;

                if (cryptoWallet == null)
                    continue;

                if (dictCryptoWalletAddressPerCurrencyPerShop.TryGetValue(shopCryptoWallet.ShopId, out Dictionary<Guid, string>? value))
                {
                    if (!value.ContainsKey(shopCryptoWallet.CryptoWalletId))
                    {
                        value[shopCryptoWallet.CryptoWalletId] = cryptoWallet.Address;
                    }
                }
                else
                {
                    dictCryptoWalletAddressPerCurrencyPerShop.Add(shopCryptoWallet.ShopId, new() { { shopCryptoWallet.CryptoWalletId, cryptoWallet.Address } });
                }
            }

            // Retrieve addresses and balances and construct dictionary
            Dictionary<string, decimal> dictBalancePerAddress = [];

            try
            {
                var listReceivedByAddress = await rpcService.ListReceivedByAddress();
                foreach (var listReceivedByAddressItem in listReceivedByAddress)
                    if (listReceivedByAddressItem.Address != null)
                        dictBalancePerAddress.Add(listReceivedByAddressItem.Address, listReceivedByAddressItem.Amount);
            }
            catch (Exception ex)
            {
                Log($"! ListReceivedByAddress error: {ex.Message}", ref sbLog);

                if (ex.StackTrace != null)
                    Log(ex.StackTrace, ref sbLog);
            }

            // Retrieve unpaid transactions and check if balance for corresponding wallets has changed
            var unpaidTransactions = await transactionRepository.GetUnpaidAndYoungerThan3Days();
            foreach (Transaction transaction in unpaidTransactions)
            {
                Log("", ref sbLog);
                Log($"Transaction {transaction.Id}", ref sbLog);
                Log($"- Recipient: {transaction.Recipient}", ref sbLog);
                Log($"- Date: {transaction.Date:dd-MM-yyyy HH:mm:ss}", ref sbLog);
                Log($"- Due: {transaction.AmountDue:N8}", ref sbLog);
                Log($"- Paid: {transaction.AmountPaid:N8}", ref sbLog);

                if (dictBalancePerAddress.TryGetValue(transaction.Recipient, out decimal value))
                {
                    decimal balance = value;

                    Log($"- Balance: {balance:N8}", ref sbLog);

                    if (balance > transaction.AmountPaid)
                    {
                        var resultTransaction = await transactionRepository.UpdateAmountPaid(transaction, balance, Guid.Empty);
                        if (resultTransaction.Success)
                            Log($"! Transaction updated", ref sbLog);
                        else
                            Log($"! Transaction update error: {resultTransaction.Message}", ref sbLog);
                    }
                }
            }

            // Retrieve orders with status 'Awaiting payment' 
            var ordersAwaitingPayment = await orderRepository.GetByStatus(Domain.Enums.OrderStatus.AwaitingPayment);
            foreach (var order in ordersAwaitingPayment)
            {
                Log("", ref sbLog);
                Log($"Order {order.Id}", ref sbLog);
                Log($"- Date: {order.Date:dd-MM-yyyy HH:mm:ss}", ref sbLog);

                if (order.Transaction != null)
                {
                    Log($"- Transaction: {order.Transaction.Id}", ref sbLog);

                    if (order.Transaction.PaidInFull.HasValue)
                    {
                        // If transaction was paid in full earlier, update order's status
                        string? merchantCryptoWalletAddress = null;

                        if (dictCryptoWalletAddressPerCurrencyPerShop.TryGetValue(order.Shop.Id!.Value, out var valueShop) && valueShop.TryGetValue(order.CurrencyId, out string? valueCryptoWallet))
                            merchantCryptoWalletAddress = valueCryptoWallet;

                        if (merchantCryptoWalletAddress != null)
                        {
                            // Send the merchant 99% of the paid amount
                            var amountToSendToMerchant = Math.Round(order.Transaction.AmountPaid * 0.99m, 8); // Round, because sendtoaddress only supports up to 8 decimals
                            string resultSendToAddress = string.Empty;

                            try
                            {
                                await rpcService.WalletPassphrase(rpcWalletPassphrase);
                            }
                            catch (Exception ex)
                            {
                                Log($"! WalletPassphrase error: {ex.Message}", ref sbLog);

                                if (ex.StackTrace != null)
                                    Log(ex.StackTrace, ref sbLog);
                            }

                            try
                            {
                                resultSendToAddress = await rpcService.SendToAddress(merchantCryptoWalletAddress, amountToSendToMerchant);
                            }
                            catch (Exception ex)
                            {
                                Log($"! SendtoAddress error: {ex.Message}", ref sbLog);

                                if (ex.StackTrace != null)
                                    Log(ex.StackTrace, ref sbLog);
                            }

                            try
                            {
                                await rpcService.WalletLock();
                            }
                            catch (Exception ex)
                            {
                                Log($"! WalletLock error: {ex.Message}", ref sbLog);

                                if (ex.StackTrace != null)
                                    Log(ex.StackTrace, ref sbLog);
                            }

                            if (!string.IsNullOrEmpty(resultSendToAddress))
                            {
                                var transactionToMerchantCreate = new Transaction()
                                {
                                    Id = Guid.NewGuid(),
                                    ShopId = order.Shop.Id!.Value,
                                    Recipient = merchantCryptoWalletAddress,
                                    AmountDue = amountToSendToMerchant,
                                    AmountPaid = amountToSendToMerchant,
                                    Tx = resultSendToAddress
                                };

                                var resultTransaction = await transactionRepository.Create(transactionToMerchantCreate, Guid.Empty);
                                if (resultTransaction.Success)
                                {
                                    Log($"! Paid merchant {amountToSendToMerchant:N8} at {merchantCryptoWalletAddress}: Your Crypto Shop Transaction {resultTransaction.Identifier} (Tx {resultSendToAddress})", ref sbLog);
                                }
                                else
                                {
                                    Log($"! Transaction creation error: {resultTransaction.Message}", ref sbLog);
                                }

                                var resultOrder = await orderRepository.UpdateStatus(order, Domain.Enums.OrderStatus.Paid, Guid.Empty);
                                if (resultOrder.Success)
                                {
                                    Log($"! Order status set to '{Domain.Enums.OrderStatus.Paid}'", ref sbLog);
                                }
                                else
                                {
                                    Log($"! Order update error: {resultOrder.Message}", ref sbLog);
                                }
                            }
                            else
                            {
                                Log($"! Unable to send funds to merchant", ref sbLog);
                            }
                        }
                        else
                        {
                            Log($"! Can not pay merchant; shop has no crypto wallet configured", ref sbLog);
                        }
                    }
                    else
                    {
                        Log($"! Order transaction is not yet paid in full", ref sbLog);

                        // If order transaction is older than 3 days, abandon order
                        if (order.Transaction.Date < DateTime.UtcNow.AddDays(-3))
                        {
                            Log($"! Order has been awaiting payment for more than 3 days.", ref sbLog);

                            var resultOrder = await orderRepository.UpdateStatus(order, Domain.Enums.OrderStatus.Abandoned, Guid.Empty);
                            if (resultOrder.Success)
                            {
                                Log($"! Order status set to '{Domain.Enums.OrderStatus.Abandoned}'", ref sbLog);
                            }
                            else
                            {
                                Log($"! Order update error: {resultOrder.Message}", ref sbLog);
                            }
                        }
                    }
                }
                else
                {
                    Log($"! Order has no transaction", ref sbLog);
                }
            }

            Log("", ref sbLog);
            Log($"End {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Create log path if it doesn't exist yet
            var logPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Log";

            if (!Path.Exists(logPath))
                Directory.CreateDirectory(logPath);

            // Delete log files older than 3 days
            string[] files = Directory.GetFiles(logPath);

            foreach (string file in files)
            {
                FileInfo fileInfo = new(file);

                if (fileInfo.CreationTime < DateTime.Now.AddDays(-3))
                    fileInfo.Delete();
            }

            // Write output to log
            Log($"Writing log to '{logPath}'", ref sbLog);
            using StreamWriter writer = new($"{logPath}/{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.log");
            writer.Write(sbLog.ToString());
        }


        private static void Log(string message, ref StringBuilder sbLog)
        {
            System.Console.WriteLine(message);
            sbLog.AppendLine(message);
        }

        private static IConfiguration GetConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!)
                .AddJsonFile("appsettings" + (env == "Development" ? ".Development" : string.Empty) + ".json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
