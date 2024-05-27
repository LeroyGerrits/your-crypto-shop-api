using YourCryptoShop.Domain.Enums;
using Newtonsoft.Json;

namespace YourCryptoShop.Domain.Models.Response
{
    public class JsonRpcError
    {
        [JsonProperty(PropertyName = "code")]
        public RpcErrorCode Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }
    }
}