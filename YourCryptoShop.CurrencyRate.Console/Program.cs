using YourCryptoShop.Data;
using YourCryptoShop.Data.Repositories;
using YourCryptoShop.Data.Services;
using YourCryptoShop.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Windows.Forms;
using YourCryptoShop.Domain.Models.Response.CryptoCompare;

namespace YourCryptoShop.BackgroundWorker
{
    internal class Program
    {
        static async Task Main()
        {
            var _configuration = GetConfig();
            string connectionString = _configuration.GetConnectionString("YourCryptoShop") ?? throw new Exception("connectionString 'DBBCommerce' not set.");

            DataAccessLayer dataAccessLayer = new(connectionString);
            CryptoCompareService cryptoCompareService = new(string.Empty);
            CurrencyRepository currencyRepository = new(dataAccessLayer);
            CurrencyRateRepository currencyRateRepository = new(dataAccessLayer);

            StringBuilder sbLog = new();
            Log($"Start {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Retrieve currencies and construct dictionary
            var currencies = await currencyRepository.Get(new Domain.Parameters.GetCurrenciesParameters());
            Dictionary<string, Currency> dictCurrencyByCode = [];
            Dictionary<Guid, Currency> dictCurrencyById = [];
            List<string> listSupportedFiat = [], listSupportedCrypto = [];

            foreach (var currency in currencies)
            {
                dictCurrencyByCode.Add(currency.Code, currency);
                dictCurrencyById.Add(currency.Id!.Value, currency);

                if (currency.Supported && currency.Type == Domain.Enums.CurrencyType.Fiat)
                    listSupportedFiat.Add(currency.Code);

                if (currency.Supported && currency.Type == Domain.Enums.CurrencyType.Crypto)
                    listSupportedCrypto.Add(currency.Code);
            }

            // Retrieve rates from CryptoCompare
            GetRatesResponse? getRatesResponse = null;

            try
            {
                getRatesResponse = await cryptoCompareService.GetRates(listSupportedFiat, listSupportedCrypto);
            }
            catch (Exception ex)
            {
                Log($"! GetRatesResponse error: {ex.Message}", ref sbLog);

                if (ex.StackTrace != null)
                    Log(ex.StackTrace, ref sbLog);
            }

            if (getRatesResponse != null)
            {
                List<CurrencyRate> currencyRates = [];

                foreach (var rate in getRatesResponse.CAD)
                    currencyRates.Add(new CurrencyRate() { CurrencyFromId = dictCurrencyByCode["CAD"].Id!.Value, CurrencyToId = dictCurrencyByCode[rate.Key].Id!.Value, Rate = rate.Value });

                foreach (var rate in getRatesResponse.EUR)
                    currencyRates.Add(new CurrencyRate() { CurrencyFromId = dictCurrencyByCode["EUR"].Id!.Value, CurrencyToId = dictCurrencyByCode[rate.Key].Id!.Value, Rate = rate.Value });

                foreach (var rate in getRatesResponse.GBP)
                    currencyRates.Add(new CurrencyRate() { CurrencyFromId = dictCurrencyByCode["GBP"].Id!.Value, CurrencyToId = dictCurrencyByCode[rate.Key].Id!.Value, Rate = rate.Value });

                foreach (var rate in getRatesResponse.USD)
                    currencyRates.Add(new CurrencyRate() { CurrencyFromId = dictCurrencyByCode["USD"].Id!.Value, CurrencyToId = dictCurrencyByCode[rate.Key].Id!.Value, Rate = rate.Value });

                foreach (var currencyRate in currencyRates)
                {
                    Log("", ref sbLog);
                    Log($"CurrencyRate", ref sbLog);
                    Log($"- From: {currencyRate.CurrencyFromId} ({dictCurrencyById[currencyRate.CurrencyFromId].Code})", ref sbLog);
                    Log($"- To: {currencyRate.CurrencyToId} ({dictCurrencyById[currencyRate.CurrencyToId].Code})", ref sbLog);
                    Log($"- Rate: {currencyRate.Rate:N8}", ref sbLog);

                    var resultCurrencyRate = await currencyRateRepository.Update(currencyRate, Guid.Empty);
                    if (!resultCurrencyRate.Success)
                        Log($"! CurrencyRate update error: {resultCurrencyRate.Message}", ref sbLog);
                }

                // TO-DO: Make DB call to update all product prices accordingly
            }
            else
            {
                Log($"! GetRatesResponse is empty.", ref sbLog);
            }

            Log("", ref sbLog);
            Log($"End {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}", ref sbLog);

            // Create log path if it doesn't exist yet
            var logPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Log";

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