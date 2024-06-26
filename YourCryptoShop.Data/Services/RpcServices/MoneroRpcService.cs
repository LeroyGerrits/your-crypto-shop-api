using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.Data.Services.RpcServices
{
    public class MoneroRpcService(string rpcUsername, string rpcPassword) : RpcService("http://127.0.0.1:18081", rpcUsername, rpcPassword), IMoneroRpcService
    {

    }
}