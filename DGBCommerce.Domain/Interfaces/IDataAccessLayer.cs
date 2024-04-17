using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<MutationResult> CreateCategory(Category category, Guid mutationId);
        Task<MutationResult> CreateCustomer(Customer customer, Guid mutationId);
        Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> CreateDeliveryMethodCostsPerCountry(DeliveryMethodCostsPerCountry deliveryMethodCostsPerCountry, Guid mutationId);
        Task<MutationResult> CreateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId);
        Task<MutationResult> CreateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> CreateMerchantPasswordResetLink(MerchantPasswordResetLink merchantPasswordResetLink);
        Task<MutationResult> CreateOrder(Order order, Guid mutationId);
        Task<MutationResult> CreateOrderItem(OrderItem orderItem, Guid mutationId);
        Task<MutationResult> CreatePage(Page page, Guid mutationId);
        Task<MutationResult> CreatePage2Category(Page2Category pageCategory, Guid mutationId);
        Task<MutationResult> CreateProduct(Product product, Guid mutationId);
        Task<MutationResult> CreateProduct2Category(Product2Category productCategory, Guid mutationId);
        Task<MutationResult> CreateProductPhoto(ProductPhoto productPhoto, Guid mutationId);
        Task<MutationResult> CreateShop(Shop shop, Guid mutationId);
        Task<MutationResult> CreateShoppingCart(ShoppingCart shoppingCart);
        Task<MutationResult> CreateShoppingCartItem(ShoppingCartItem shoppingCartItem);
        Task<MutationResult> CreateTransaction(Transaction transaction, Guid mutationId);

        Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteCustomer(Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId);
        Task<MutationResult> DeleteDeliveryMethodCostsPerCountry(Guid deliveryMethodId, Guid countryId, Guid mutationId);
        Task<MutationResult> DeleteDigiByteWallet(Guid digiByteWalletId, Guid mutationId);
        Task<MutationResult> DeleteOrder(Guid orderId, Guid mutationId);
        Task<MutationResult> DeleteOrderItem(Guid orderItemId, Guid mutationId);
        Task<MutationResult> DeletePage(Guid pageId, Guid mutationId);
        Task<MutationResult> DeletePage2Category(Guid pageId, Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteProduct(Guid productId, Guid mutationId);
        Task<MutationResult> DeleteProduct2Category(Guid productId, Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteProductPhoto(Guid productPhotoId, Guid mutationId);
        Task<MutationResult> DeleteShop(Guid shopId, Guid mutationId);
        Task<MutationResult> DeleteShoppingCartItem(Guid shoppingCartItemId);

        Task<DataTable> GetAddress(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryId);
        Task<DataTable> GetCategories(GetCategoriesParameters parameters);
        Task<DataTable> GetCustomers(GetCustomersParameters parameters);
        Task<DataTable> GetCustomerByEmailAddress(Guid shopId, string emailAddress);
        Task<DataTable> GetCustomerByEmailAddressAndPassword(Guid shopId, string emailAddress, string password, string? ipAddress);
        Task<DataTable> GetCustomerByIdAndPassword(Guid id, string password);
        Task<DataTable> GetCountries(GetCountriesParameters parameters);
        Task<DataTable> GetCurrencies(GetCurrenciesParameters parameters);
        Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters);
        Task<DataTable> GetDeliveryMethodCostsPerCountry(GetDeliveryMethodCostsPerCountryParameters parameters);
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
        Task<DataTable> GetOrders(GetOrdersParameters parameters);
        Task<DataTable> GetOrderItems(GetOrderItemsParameters parameters);
        Task<DataTable> GetPages(GetPagesParameters parameters);
        Task<DataTable> GetPage2Categories(GetPage2CategoriesParameters parameters);
        Task<DataTable> GetPageCategories(GetPageCategoriesParameters parameters);
        Task<DataTable> GetProducts(GetProductsParameters parameters);
        Task<DataTable> GetProduct2Categories(GetProduct2CategoriesParameters parameters);
        Task<DataTable> GetProductPhotos(GetProductPhotosParameters parameters);
        Task<DataTable> GetShopCategories(GetShopCategoriesParameters parameters);
        Task<DataTable> GetShops(GetShopsParameters parameters);
        Task<DataTable> GetShopByIdAndSubDomain(Guid? id, string subDomain);
        Task<DataTable> GetShoppingCarts(GetShoppingCartsParameters parameters);
        Task<DataTable> GetShoppingCartItems(GetShoppingCartItemsParameters parameters);
        Task<DataTable> GetStats();
        Task<DataTable> GetTransactions(GetTransactionsParameters parameters);

        Task<MutationResult> UpdateCategory(Category category, Guid mutationId);
        Task<MutationResult> UpdateCategoryChangeParent(Guid categoryId, Guid parentId, Guid mutationId);
        Task<MutationResult> UpdateCategoryMoveDown(Guid categoryId, Guid? parentId, Guid mutationId);
        Task<MutationResult> UpdateCategoryMoveUp(Guid categoryId, Guid? parentId, Guid mutationId);
        Task<MutationResult> UpdateCustomer(Customer customer, Guid mutationId);
        Task<MutationResult> UpdateCustomerPasswordAndActivate(Customer customer, string password, Guid mutationId);
        Task<MutationResult> UpdateCustomerPasswordAndSalt(Customer customer, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> UpdateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId);
        Task<MutationResult> UpdateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordAndActivate(Merchant merchant, string password, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordAndSalt(Merchant merchant, string password, string passwordSalt, Guid mutationId);
        Task<MutationResult> UpdateMerchantPasswordResetLinkUsed(MerchantPasswordResetLink merchantPasswordResetLink, Guid mutationId);
        Task<MutationResult> UpdateOrder(Order order, Guid mutationId);
        Task<MutationResult> UpdateOrderStatus(Guid orderId, OrderStatus status, Guid mutationId);
        Task<MutationResult> UpdateOrderTransaction(Guid orderId, Guid transactionId, Guid mutationId);
        Task<MutationResult> UpdateOrderItem(OrderItem orderItem, Guid mutationId);
        Task<MutationResult> UpdatePage(Page page, Guid mutationId);
        Task<MutationResult> UpdateProduct(Product product, Guid mutationId);
        Task<MutationResult> UpdateProductPhoto(ProductPhoto productPhoto, Guid mutationId);
        Task<MutationResult> UpdateProductPhotoChangeDescription(Guid productPhotoId, string description, Guid mutationId);
        Task<MutationResult> UpdateProductPhotoChangeMain(Guid productPhotoId, Guid productId, Guid mutationId);
        Task<MutationResult> UpdateProductPhotoChangeVisible(Guid productPhotoId, bool visible, Guid mutationId);
        Task<MutationResult> UpdateProductPhotoMoveDown(Guid productPhotoId, Guid productId, Guid mutationId);
        Task<MutationResult> UpdateProductPhotoMoveUp(Guid productPhotoId, Guid productId, Guid mutationId);
        Task<MutationResult> UpdateShop(Shop shop, Guid mutationId);
        Task<MutationResult> UpdateShoppingCartItem(ShoppingCartItem shoppingCartItem);
        Task<MutationResult> UpdateTransactionAmountPaid(Guid transactionId, decimal amountPaid, Guid mutationId);
    }
}