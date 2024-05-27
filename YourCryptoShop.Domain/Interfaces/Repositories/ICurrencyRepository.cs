using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface ICurrencyRepository : IPublicRepository<Currency, GetCurrenciesParameters> { }
}