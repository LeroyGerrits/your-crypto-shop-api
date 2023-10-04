using DGBCommerce.Domain.Enums;
using Newtonsoft.Json;

namespace DGBCommerce.Domain.Models.Response
{
    public class JsonRpcError
    {
        [JsonProperty(PropertyName = "code")]
        public RpcErrorCode Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }
    }
}