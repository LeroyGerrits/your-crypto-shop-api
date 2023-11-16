using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<MutationResult> CreateCategory(Category category, Guid mutationId);
        Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> CreateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId);
        Task<MutationResult> CreateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> CreateMerchantPasswordResetLink(MerchantPasswordResetLink merchantPasswordResetLink);
        Task<MutationResult> CreateProduct(Product product, Guid mutationId);
        Task<MutationResult> CreateProductCategory(ProductCategory productCategory, Guid mutationId);
        Task<MutationResult> CreateProductPhoto(ProductPhoto productPhoto, Guid mutationId);
        Task<MutationResult> CreateShop(Shop shop, Guid mutationId);

        Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId);
        Task<MutationResult> DeleteDigiByteWallet(Guid digiByteWalletId, Guid mutationId);
        Task<MutationResult> DeleteProduct(Guid productId, Guid mutationId);
        Task<MutationResult> DeleteProductCategory(Guid productId, Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteProductPhoto(Guid productPhotoId, Guid mutationId);
        Task<MutationResult> DeleteShop(Guid shopId, Guid mutationId);

        Task<DataTable> GetCategories(GetCategoriesParameters parameters);
        Task<DataTable> GetCountries(GetCountriesParameters parameters);
        Task<DataTable> GetCurrencies(GetCurrenciesParameters parameters);
        Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters);
        Task<DataTable> GetDigiByteWallets(GetDigiByteWalletsParameters parameters);
        Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters);
        Task<DataTable> GetFaqs(GetFaqsParameters parameters);
        Task<DataTable> GetFinancialStatementTransactions(GetFinancialStatementTransactionsParameters parameters);
        Task<DataTable> GetMerchantByEmailAddress(string emailAddress);
        Task<DataTable> GetMerchantByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress);
        Task<DataTable> GetMerchantByIdAndPassword(Guid id, string password);
        Task<DataTable> GetMerchantPasswordResetLinkByIdAndKey(Guid id, string key);
        Task<DataTable> GetMerchants(GetMerchantsParameters parameters);
        Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters);
        Task<DataTable> GetProducts(GetProductsParameters parameters);
        Task<DataTable> GetProductCategories(GetProductCategoriesParameters parameters);
        Task<DataTable> GetProductPhotos(GetProductPhotosParameters parameters);
        Task<DataTable> GetShopCategories(GetShopCategoriesParameters parameters);
        Task<DataTable> GetShops(GetShopsParameters parameters);
        Task<DataTable> GetShopByIdAndSubDomain(Guid? id, string subDomain);

        Task<MutationResult> UpdateCategory(Category category, Guid mutationId);
        Task<MutationResult> UpdateCategoryChangeParent(Guid categoryId, Guid parentId, Guid mutationId);
        Task<MutationResult> UpdateCategoryMoveDown(Guid categoryId, Guid? parentId, Guid mutationId);
        Task<MutationResult> UpdateCategoryMoveUp(Guid categoryId, Guid? parentId, Guid mutationId);
        Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> UpdateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId);
        Task<MutationResult> UpdateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordAndActivate(Merchant merchant, string password, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordAndSalt(Merchant merchant, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordResetLinkUsed(MerchantPasswordResetLink merchantPasswordResetLink, Guid mutationId);
        Task<MutationResult> UpdateProduct(Product product, Guid mutationId);
        Task<MutationResult> UpdateProductPhoto(ProductPhoto productPhoto, Guid mutationId);
        Task<MutationResult> UpdateShop(Shop shop, Guid mutationId);
    }
}