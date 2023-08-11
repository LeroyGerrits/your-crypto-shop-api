using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid merchantId);
        Task<MutationResult> CreateMerchant(Merchant merchant, Guid merchantId);
        Task<MutationResult> CreateShop(Shop shop, Guid merchantId);

        Task<DataTable> GetCategories(GetCategoriesParameters parameters);
        Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters);
        Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters);
        Task<DataTable> GetFaqs(GetFaqsParameters parameters);
        Task<DataTable> GetMerchants(GetMerchantsParameters parameters);
        Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters);
        Task<DataTable> GetShops(GetShopsParameters parameters);
    }
}