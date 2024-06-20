using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.Data.Services.RpcServices
{
    public class DigiByteRpcService(string rpcUsername, string rpcPassword) : RpcService("http://127.0.0.1:14022", rpcUsername, rpcPassword), IDigiByteRpcService
    {

    }
}