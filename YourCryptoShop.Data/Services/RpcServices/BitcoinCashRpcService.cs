using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.Data.Services.RpcServices
{
    public class BitcoinCashRpcService(string rpcUsername, string rpcPassword) : RpcService("http://127.0.0.1:8332", rpcUsername, rpcPassword), IBitcoinCashRpcService
    {

    }
}