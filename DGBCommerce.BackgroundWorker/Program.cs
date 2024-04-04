using DGBCommerce.Data;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Data.Services;
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

            DataAccessLayer dataAccessLayer = new(connectionString);
            OrderRepository orderRepository = new(dataAccessLayer);
            RpcService rpcService = new(rpcDaemonUrl, rpcUsername, rpcPassword);
            TransactionRepository transactionRepository = new(dataAccessLayer);

            StringBuilder sbLog = new();
            Log($"Start {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);
            
            // Retrieve addresses and balances and construct dictionary
            var listReceivedByAddress = await rpcService.ListReceivedByAddress();
            Dictionary<string, decimal> dictBalancePerAddress = [];

            foreach (var listReceivedByAddressItem in listReceivedByAddress)
                if (listReceivedByAddressItem.Address != null)
                    dictBalancePerAddress.Add(listReceivedByAddressItem.Address, listReceivedByAddressItem.Amount);

            // Retrieve unpaid transactions and check if balance for corresponding wallets has changed
            var unpaidTransactions = await transactionRepository.GetUnpaid();
            foreach (Domain.Models.Transaction transaction in unpaidTransactions)
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
                            Log($"! Updated", ref sbLog);
                        else
                            Log($"! Update error: {resultTransaction.Message}", ref sbLog);
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

                    // If transaction was paid in full earlier, update order's status
                    if (order.Transaction.PaidInFull.HasValue)
                    {
                        var resultOrder = await orderRepository.UpdateStatus(order, Domain.Enums.OrderStatus.Paid, Guid.Empty);
                        if (resultOrder.Success)
                            Log($"! Updated", ref sbLog);
                        else
                            Log($"! Update error: {resultOrder.Message}", ref sbLog);
                    }
                    else
                    {
                        // Set order to abandoned if it has been awaiting payment for x days?
                    }
                }
            }

            Log("", ref sbLog);
            Log($"End {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Write output to log
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Log";
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
