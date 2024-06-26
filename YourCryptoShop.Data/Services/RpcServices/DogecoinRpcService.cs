using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.Data.Services.RpcServices
{
    public class DogecoinRpcService(string rpcUsername, string rpcPassword) : RpcService("http://127.0.0.1:22555", rpcUsername, rpcPassword), IDogecoinRpcService
    {

    }
}