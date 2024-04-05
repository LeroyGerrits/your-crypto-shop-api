using DGBCommerce.Data;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Data.Services;
using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace DGBCommerce.BackgroundWorker
{
    internal class Program
    {
        static async Task Main()
        {
            var _configuration = GetConfig();
            string connectionString = _configuration.GetConnectionString("DGBCommerce") ?? throw new Exception("connectionString 'DBBCommerce' not set.");
            string rpcDaemonUrl = _configuration.GetSection("RpcSettings:DaemonUrl").Value ?? throw new Exception("RPC setting 'DaemonUrl' not set.");
            string rpcUsername = _configuration.GetSection("RpcSettings:Username").Value ?? throw new Exception("RPC setting 'Username' not set.");
            string rpcPassword = _configuration.GetSection("RpcSettings:Password").Value ?? throw new Exception("RPC setting 'Password' not set.");
            string rpcWalletPassphrase = _configuration.GetSection("RpcSettings:WalletPassphrase").Value ?? throw new Exception("RPC setting 'WalletPassphrase' not set.");

            DataAccessLayer dataAccessLayer = new(connectionString);
            OrderRepository orderRepository = new(dataAccessLayer);
            RpcService rpcService = new(rpcDaemonUrl, rpcUsername, rpcPassword);
            ShopRepository shopRepository = new(dataAccessLayer);
            TransactionRepository transactionRepository = new(dataAccessLayer);

            StringBuilder sbLog = new();
            Log($"Start {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Retrieve shops and DigiByte wallets and construct dictionary
            var shops = await shopRepository.Get(new Domain.Parameters.GetShopsParameters());
            Dictionary<Guid, string> dictDigiByteWalletPerShop = [];

            foreach (var shop in shops)
                if (shop.Wallet != null)
                    dictDigiByteWalletPerShop.Add(shop.Id!.Value, shop.Wallet.Address);

            // Retrieve addresses and balances and construct dictionary
            var listReceivedByAddress = await rpcService.ListReceivedByAddress();
            Dictionary<string, decimal> dictBalancePerAddress = [];

            foreach (var listReceivedByAddressItem in listReceivedByAddress)
                if (listReceivedByAddressItem.Address != null)
                    dictBalancePerAddress.Add(listReceivedByAddressItem.Address, listReceivedByAddressItem.Amount);

            // Retrieve unpaid transactions and check if balance for corresponding wallets has changed
            var unpaidTransactions = await transactionRepository.GetUnpaid();
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

                if (order.Transaction != null)
                {
                    Log($"- Transaction: {order.Transaction.Id}", ref sbLog);

                    if (order.Transaction.PaidInFull.HasValue)
                    {
                        // If transaction was paid in full earlier, update order's status
                        string? merchantDigiByteWalletAddress = null;

                        if (dictDigiByteWalletPerShop.TryGetValue(order.ShopId, out var value))
                            merchantDigiByteWalletAddress = value;

                        if (merchantDigiByteWalletAddress != null)
                        {
                            // Send the merchant 99% of the paid amount
                            var amountToSendToMerchant = Math.Round(order.Transaction.AmountPaid * 0.99m, 8); // sendtoaddress only supports up to 8 decimals
                            string resultSendToAddress = string.Empty;

                            try
                            {
                                await rpcService.WalletPassphrase(rpcWalletPassphrase);
                                resultSendToAddress = await rpcService.SendToAddress(merchantDigiByteWalletAddress, amountToSendToMerchant);
                                await rpcService.WalletLock();
                            }
                            catch (Exception ex)
                            {
                                Log($"! SendtoAddress error: {ex.Message}", ref sbLog);
                            }

                            if (!string.IsNullOrEmpty(resultSendToAddress))
                            {
                                var transactionToCreate = new Transaction()
                                {
                                    ShopId = order.ShopId,
                                    Recipient = merchantDigiByteWalletAddress,
                                    AmountDue = amountToSendToMerchant,
                                    AmountPaid = amountToSendToMerchant,
                                    Tx = resultSendToAddress
                                };

                                var resultTransaction = await transactionRepository.Create(transactionToCreate, Guid.Empty);
                                if (resultTransaction.Success)
                                {
                                    Log($"! Paid merchant {amountToSendToMerchant:N8} at {merchantDigiByteWalletAddress}: DGB Commerce Transaction {resultTransaction.Identifier} (Tx {resultSendToAddress})", ref sbLog);
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
                                Log($"! Can not pay merchant; shop has no DigiByte wallet configured", ref sbLog);
                            }
                        }
                        else
                        {
                            // Set order to abandoned if it has been awaiting payment for x days?
                        }
                    }
                    else
                    {
                        Log($"! Order transaction is not yet paid in full", ref sbLog);
                    }
                }
                else
                {
                    Log($"! Order has no transaction", ref sbLog);
                }
            }

            Log("", ref sbLog);
            Log($"End {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Write output to log
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Log";
            Log($"Writing log to '{path}'", ref sbLog);

            if (!Path.Exists(path))
                Directory.CreateDirectory(path);

            using StreamWriter writer = new($"{path}/{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.log");
            writer.Write(sbLog.ToString());
        }


        private static void Log(string message, ref StringBuilder sbLog)
        {
            Console.WriteLine(message);
            sbLog.AppendLine(message);
        }

        private static IConfiguration GetConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings" + (env == "Development" ? ".Development" : string.Empty) + ".json", optional: false, reloadOnChange: true); ;

            return builder.Build();
        }
    }
}
