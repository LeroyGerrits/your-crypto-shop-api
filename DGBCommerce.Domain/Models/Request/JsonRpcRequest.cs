using System.Text;
using Newtonsoft.Json;

namespace DGBCommerce.Domain.Models.Request
{
    public class JsonRpcRequest
    {
        public JsonRpcRequest(int id, string method, params object?[] parameters)
        {
            Id = id;
            Method = method;
            Parameters = parameters?.ToList() ?? new List<object?>();
        }

        [JsonProperty(PropertyName = "method", Order = 0)]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "params", Order = 1)]
        public IList<object?> Parameters { get; set; }

        [JsonProperty(PropertyName = "id", Order = 2)]
        public int Id { get; set; }

        public string GetRaw()
            => JsonConvert.SerializeObject(this);

        public byte[] GetBytes()
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}