using YourCryptoShop.Domain.Interfaces.Services;
using YourCryptoShop.Domain.Models.Request;
using YourCryptoShop.Domain.Models.Response;
using Newtonsoft.Json;
using YourCryptoShop.Domain.Models.Response.CryptoCompare;
using System.Net;
using System.Text;
using YourCryptoShop.Domain.Exceptions;

namespace YourCryptoShop.Data.Services
{
    public class CryptoCompareService(string apiKey) : ICryptoCompareService
    {
        private readonly string _baseUrl = "https://min-api.cryptocompare.com/data/pricemulti?fsyms=BCH,DCR,DOGE,LTC,XMR&tsyms=USD,EUR,CAD,GBP";
        private readonly string _apiKey = apiKey;

        public async Task<GetRatesResponse> GetRates()
        {
            HttpClient httpClient = new();
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, _baseUrl);
            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
            catch
            {
                // error handling
                throw;
            }

            string responseBody;
            using (StreamReader streamReaderResponse = new(httpResponseMessage.Content.ReadAsStream()))
                responseBody = streamReaderResponse.ReadToEnd();

            GetRatesResponse? getRatesResponse;

            try
            {
                getRatesResponse = JsonConvert.DeserializeObject<GetRatesResponse>(responseBody);
            }
            catch (JsonException jsonException)
            {
                throw new Exception("There was a problem deserializing the response.", jsonException);
            }

            if (getRatesResponse == null)
                throw new Exception("Response was empty.");

            return getRatesResponse;
        }
    }
}