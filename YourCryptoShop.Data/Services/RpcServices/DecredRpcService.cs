using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.Data.Services.RpcServices
{
    public class DecredRpcService(string rpcUsername, string rpcPassword) : RpcService("http://127.0.0.1:9109", rpcUsername, rpcPassword), IDecredRpcService
    {

    }
}