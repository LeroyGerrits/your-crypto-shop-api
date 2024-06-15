using YourCryptoShop.Data;
using YourCryptoShop.Data.Repositories;
using YourCryptoShop.Data.Services;
using YourCryptoShop.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Windows.Forms;

namespace YourCryptoShop.BackgroundWorker
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
            RpcService rpcService = new(rpcDaemonUrl, rpcUsername, rpcPassword);
            CurrencyRepository currencyRepository = new(dataAccessLayer);
            TransactionRepository transactionRepository = new(dataAccessLayer);

            StringBuilder sbLog = new();
            Log($"Start {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Retrieve currencies and construct dictionary
            var currencies = await currencyRepository.Get(new Domain.Parameters.GetCurrenciesParameters());
            Dictionary<Guid, Currency> dictCurrencies = [];

            foreach (var currency in currencies)
                dictCurrencies.Add(currency.Id!.Value, currency);

            // Retrieve rates from CryptoCompare
            var getRatesResponse = 

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
                        string? merchantDigiByteWalletAddress = null;

                        if (dictDigiByteWalletPerShop.TryGetValue(order.Shop.Id!.Value, out var value))
                            merchantDigiByteWalletAddress = value;

                        if (merchantDigiByteWalletAddress != null)
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
                                resultSendToAddress = await rpcService.SendToAddress(merchantDigiByteWalletAddress, amountToSendToMerchant);
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
                                    Recipient = merchantDigiByteWalletAddress,
                                    AmountDue = amountToSendToMerchant,
                                    AmountPaid = amountToSendToMerchant,
                                    Tx = resultSendToAddress
                                };

                                var resultTransaction = await transactionRepository.Create(transactionToMerchantCreate, Guid.Empty);
                                if (resultTransaction.Success)
                                {
                                    Log($"! Paid merchant {amountToSendToMerchant:N8} at {merchantDigiByteWalletAddress}: Your Crypto Shop Transaction {resultTransaction.Identifier} (Tx {resultSendToAddress})", ref sbLog);
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
                            Log($"! Can not pay merchant; shop has no DigiByte wallet configured", ref sbLog);
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

            var logPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Log";

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

            if (!Path.Exists(logPath))
                Directory.CreateDirectory(logPath);

            using StreamWriter writer = new($"{logPath}/{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.log");
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
                .SetBasePath(Path.GetDirectoryName(Application.ExecutablePath)!)
                .AddJsonFile("appsettings" + (env == "Development" ? ".Development" : string.Empty) + ".json", optional: false, reloadOnChange: true);

            return builder.Build();
        }
    }
}
