using DGBCommerce.Data;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace DGBCommerce.BackgroundWorker
{
    internal class Program
    {
        static async Task Main()
        {
            var _configuration = GetConfig();
            string connectionString = _configuration.GetConnectionString("DGBCommerce") ?? throw new Exception("connectionString 'DBBCommerce' not set.");
            DataAccessLayer dataAccessLayer = new(connectionString);
            MerchantRepository merchantRepository = new(dataAccessLayer);

            Console.WriteLine("ConnectionString:");
            Console.WriteLine(connectionString);
            Console.WriteLine();

            // Show merchant list as test for now
            var merchants = await merchantRepository.Get(new Domain.Parameters.GetMerchantsParameters());
            foreach (Merchant merchant in merchants)
            {
                Console.WriteLine(merchant.Username);
            }

            Console.ReadLine();
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
