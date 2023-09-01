using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Domain.Interfaces
{
    public interface ICurrencyRepository : IPublicRepository<Currency, GetCurrenciesParameters> { }
}