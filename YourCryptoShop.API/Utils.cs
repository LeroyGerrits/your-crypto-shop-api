using YourCryptoShop.Domain.Interfaces.Services.RpcServices;

namespace YourCryptoShop.API
{
    public interface IUtils
    {
        public IRpcService GetRpcService(string coin);
    }

    public class Utils(
        IBitcoinCashRpcService bitcoinCashRpcService,
        IDecredRpcService decredRpcService,
        IDogecoinRpcService dogecoinRpcService,
        ILitecoinRpcService litecoinRpcService,
        IMoneroRpcService moneroRpcService
        ) : IUtils
    {
        public IRpcService GetRpcService(string coin)
        {
            return coin switch
            {
                "BCH" => bitcoinCashRpcService,
                "DCR" => decredRpcService,
                "DOGE" => dogecoinRpcService,
                "LTC" => litecoinRpcService,
                "XMR" => moneroRpcService,
                _ => throw new Exception($"Unknown coin '{coin}'. Can not initialize corresponding RPC service."),
            };
        }
    }
}