using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<MutationResult> CreateCategory(Category category, Guid mutationId);
        Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> CreateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> CreateShop(Shop shop, Guid mutationId);

        Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId);

        Task<DataTable> GetCategories(GetCategoriesParameters parameters);
        Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters);
        Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters);
        Task<DataTable> GetFaqs(GetFaqsParameters parameters);
        Task<DataTable> GetMerchantForForgotPassword(GetMerchantForForgotPasswordParameters parameters);
        Task<DataTable> GetMerchantForLogin(GetMerchantForLoginParameters parameters);
        Task<DataTable> GetMerchants(GetMerchantsParameters parameters);
        Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters);
        Task<DataTable> GetShops(GetShopsParameters parameters);

        Task<MutationResult> UpdateCategory(Category category, Guid mutationId);
        Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
    }
}