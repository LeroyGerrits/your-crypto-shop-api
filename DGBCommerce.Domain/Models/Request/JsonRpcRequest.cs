using System.Text;
using Newtonsoft.Json;

namespace DGBCommerce.Domain.Models.Request
{
    public class JsonRpcRequest(int id, string method, params object?[] parameters)
    {
        [JsonProperty(PropertyName = "method", Order = 0)]
        public string Method { get; set; } = method;

        [JsonProperty(PropertyName = "params", Order = 1)]
        public IList<object?> Parameters { get; set; } = parameters?.ToList() ?? [];

        [JsonProperty(PropertyName = "id", Order = 2)]
        public int Id { get; set; } = id;

        public string GetRaw()
            => JsonConvert.SerializeObject(this);

        public byte[] GetBytes()
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
    }
}