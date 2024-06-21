using YourCryptoShop.Domain.Interfaces.Services;
using Newtonsoft.Json;
using YourCryptoShop.Domain.Models.Response.CryptoCompare;

namespace YourCryptoShop.Data.Services
{
    public class CryptoCompareService : ICryptoCompareService
    {
        private readonly string _baseUrl = "https://min-api.cryptocompare.com";
        
        public async Task<GetRatesResponse> GetRates(List<string> from, List<string> to)
        {
            HttpClient httpClient = new();
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Get, $"{_baseUrl}/data/pricemulti?fsyms={string.Join(',', from)}&tsyms={string.Join(',', to)}");
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