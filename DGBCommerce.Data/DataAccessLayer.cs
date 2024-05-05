using System.Data;
using Microsoft.Data.SqlClient;
using DGBCommerce.Domain.Parameters;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;

namespace DGBCommerce.Data
{
    public class DataAccessLayer(string connectionString) : IDataAccessLayer
    {
        private readonly string _connectionString = connectionString;

        #region Create
        public async Task<MutationResult> CreateCategory(Category category, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.ShopId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_VISIBLE", SqlDbType.Bit) { Value = category.Visible }
            ], mutationId);

        public async Task<MutationResult> CreateCustomer(Customer customer, Guid mutationId)
            => await NonQuery("SP_MUTATE_Customer", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@CUS_SHOP", SqlDbType.UniqueIdentifier) { Value = customer.ShopId },
                new SqlParameter("@CUS_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = customer.EmailAddress },
                new SqlParameter("@CUS_USERNAME", SqlDbType.VarChar) { Value = customer.Username },
                new SqlParameter("@CUS_PASSWORD_SALT", SqlDbType.VarChar) { Value = customer.PasswordSalt },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar) { Value = customer.Password },
                new SqlParameter("@CUS_GENDER", SqlDbType.TinyInt) { Value = customer.Gender },
                new SqlParameter("@CUS_FIRST_NAME", SqlDbType.NVarChar) { Value = customer.FirstName },
                new SqlParameter("@CUS_LAST_NAME", SqlDbType.NVarChar) { Value = customer.LastName },
                new SqlParameter("@CUS_ADDRESS", SqlDbType.UniqueIdentifier) { Value = customer.Address!.Id }
            ], mutationId);

        public async Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Shop.Id },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar, 255) { Value = deliveryMethod.Name },
                new SqlParameter("@DLM_COSTS", SqlDbType.Decimal) { Value = deliveryMethod.Costs }
            ], mutationId);

        public async Task<MutationResult> CreateDeliveryMethodCostsPerCountry(DeliveryMethodCostsPerCountry deliveryMethodCostsPerCountry, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethodCostsPerCountry", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@CPC_DELIVERY_METHOD", SqlDbType.UniqueIdentifier) { Value = deliveryMethodCostsPerCountry.DeliveryMethodId },
                new SqlParameter("@CPC_COUNTRY", SqlDbType.UniqueIdentifier) { Value = deliveryMethodCostsPerCountry.CountryId },
                new SqlParameter("@CPC_COSTS", SqlDbType.Decimal) { Value = deliveryMethodCostsPerCountry.Costs }
            ], mutationId);

        public async Task<MutationResult> CreateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.MerchantId },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar, 255) { Value = digiByteWallet.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar, 100) { Value = digiByteWallet.Address }
            ], mutationId);

        public async Task<MutationResult> CreateMerchant(Merchant merchant, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = merchant.EmailAddress },
                new SqlParameter("@MER_USERNAME", SqlDbType.VarChar) { Value = merchant.Username },
                new SqlParameter("@MER_PASSWORD_SALT", SqlDbType.VarChar) { Value = merchant.PasswordSalt },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = merchant.Password },
                new SqlParameter("@MER_GENDER", SqlDbType.TinyInt) { Value = merchant.Gender },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar) { Value = merchant.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar) { Value = merchant.LastName }
            ], mutationId);

        public async Task<MutationResult> CreateMerchantPasswordResetLink(MerchantPasswordResetLink merchantPasswordResetLink)
            => await NonQuery("SP_MUTATE_MerchantPasswordResetLink", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PRL_MERCHANT", SqlDbType.UniqueIdentifier) { Value = merchantPasswordResetLink.Merchant.Id },
                new SqlParameter("@PRL_IP_ADDRESS", SqlDbType.VarChar) { Value = merchantPasswordResetLink.IpAddress },
                new SqlParameter("@PRL_KEY", SqlDbType.VarChar) { Value = merchantPasswordResetLink.Key }
            ]);

        public async Task<MutationResult> CreateOrder(Order order, Guid mutationId)
            => await NonQuery("SP_MUTATE_Order", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@ORD_SHOP", SqlDbType.UniqueIdentifier) { Value = order.Shop.Id },
                new SqlParameter("@ORD_CUSTOMER", SqlDbType.UniqueIdentifier) { Value = order.Customer.Id },
                new SqlParameter("@ORD_STATUS", SqlDbType.TinyInt) { Value = order.Status },
                new SqlParameter("@ORD_ADDRESS_BILLING", SqlDbType.UniqueIdentifier) { Value = order.BillingAddress.Id },
                new SqlParameter("@ORD_ADDRESS_SHIPPING", SqlDbType.UniqueIdentifier) { Value = order.ShippingAddress.Id },
                new SqlParameter("@ORD_DELIVERY_METHOD", SqlDbType.UniqueIdentifier) { Value = order.DeliveryMethodId },
                new SqlParameter("@ORD_COMMENTS", SqlDbType.NVarChar) { Value = order.Comments },
                new SqlParameter("@ORD_SENDER_WALLET_ADDRESS", SqlDbType.VarChar) { Value = order.SenderWalletAddress }
            ], mutationId);

        public async Task<MutationResult> CreateOrderItem(OrderItem orderItem, Guid mutationId)
            => await NonQuery("SP_MUTATE_OrderItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@ORI_ORDER", SqlDbType.UniqueIdentifier) { Value = orderItem.OrderId },
                new SqlParameter("@ORI_TYPE", SqlDbType.TinyInt) { Value = orderItem.Type },
                new SqlParameter("@ORI_PRODUCT", SqlDbType.UniqueIdentifier) { Value = orderItem.ProductId },
                new SqlParameter("@ORI_AMOUNT", SqlDbType.Int) { Value = orderItem.Amount },
                new SqlParameter("@ORI_PRICE", SqlDbType.Decimal) { Value = orderItem.Price },
                new SqlParameter("@ORI_DESCRIPTION", SqlDbType.NVarChar) { Value = orderItem.Description }
            ], mutationId);

        public async Task<MutationResult> CreatePage(Page page, Guid mutationId)
            => await NonQuery("SP_MUTATE_Page", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PAG_SHOP", SqlDbType.UniqueIdentifier) { Value = page.Shop.Id },
                new SqlParameter("@PAG_TITLE", SqlDbType.NVarChar) { Value = page.Title },
                new SqlParameter("@PAG_CONTENT", SqlDbType.NVarChar) { Value = page.Content },
                new SqlParameter("@PAG_VISIBLE", SqlDbType.Bit) { Value = page.Visible },
                new SqlParameter("@PAG_INDEX", SqlDbType.Bit) { Value = page.Index }
            ], mutationId);

        public async Task<MutationResult> CreatePage2Category(Page2Category pageCategory, Guid mutationId)
            => await NonQuery("SP_MUTATE_Page2Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@P2C_PAGE", SqlDbType.UniqueIdentifier) { Value = pageCategory.PageId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = pageCategory.CategoryId }
            ], mutationId);

        public async Task<MutationResult> CreateProduct(Product product, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = product.ShopId },
                new SqlParameter("@PRD_CODE", SqlDbType.NVarChar) { Value = product.Code },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = product.Name },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = product.Description },
                new SqlParameter("@PRD_STOCK", SqlDbType.Int) { Value = product.Stock },
                new SqlParameter("@PRD_PRICE", SqlDbType.Decimal) { Value = product.Price },
                new SqlParameter("@PRD_VISIBLE", SqlDbType.Bit) { Value = product.Visible },
                new SqlParameter("@PRD_SHOW_ON_HOME", SqlDbType.Bit) { Value = product.ShowOnHome }
            ], mutationId);

        public async Task<MutationResult> CreateProduct2Category(Product2Category productCategory, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product2Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productCategory.ProductId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = productCategory.CategoryId }
            ], mutationId);

        public async Task<MutationResult> CreateProductPhoto(ProductPhoto productPhoto, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhoto.Id },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productPhoto.ProductId },
                new SqlParameter("@PHT_FILE", SqlDbType.VarChar) { Value = productPhoto.File },
                new SqlParameter("@PHT_EXTENSION", SqlDbType.VarChar) { Value = productPhoto.Extension },
                new SqlParameter("@PHT_FILE_SIZE", SqlDbType.Int) { Value = productPhoto.FileSize },
                new SqlParameter("@PHT_WIDTH", SqlDbType.Int) { Value = productPhoto.Width },
                new SqlParameter("@PHT_HEIGHT", SqlDbType.Int) { Value = productPhoto.Height },
                new SqlParameter("@PHT_DESCRIPTION", SqlDbType.NVarChar) { Value = productPhoto.Description },
                new SqlParameter("@PHT_SORTORDER", SqlDbType.Int) { Value = productPhoto.SortOrder },
                new SqlParameter("@PHT_MAIN", SqlDbType.Bit) { Value = productPhoto.Main },
                new SqlParameter("@PHT_VISIBLE", SqlDbType.Bit) { Value = productPhoto.Visible }
            ], mutationId);

        public async Task<MutationResult> CreateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar) { Value = shop.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.VarChar) { Value = shop.SubDomain },
                new SqlParameter("@SHP_COUNTRY", SqlDbType.UniqueIdentifier) { Value = shop.Country?.Id },
                new SqlParameter("@SHP_CATEGORY", SqlDbType.UniqueIdentifier) { Value = shop.Category?.Id },
                new SqlParameter("@SHP_WALLET", SqlDbType.UniqueIdentifier) { Value = shop.Wallet?.Id },
                new SqlParameter("@SHP_ORDER_METHOD", SqlDbType.TinyInt) { Value = shop.OrderMethod },
                new SqlParameter("@SHP_REQUIRE_ADDRESSES", SqlDbType.Bit) { Value = shop.RequireAddresses }
            ], mutationId);

        public async Task<MutationResult> CreateShoppingCart(ShoppingCart shoppingCart)
            => await NonQuery("SP_MUTATE_ShoppingCart", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@SHC_SESSION", SqlDbType.UniqueIdentifier) { Value = shoppingCart.Session },
                new SqlParameter("@SHC_CUSTOMER", SqlDbType.UniqueIdentifier) { Value = shoppingCart.CustomerId },
                new SqlParameter("@SHC_LAST_IP_ADDRESS", SqlDbType.VarChar) { Value = shoppingCart.LastIpAddress }
            ]);

        public async Task<MutationResult> CreateShoppingCartItem(ShoppingCartItem shoppingCartItem)
            => await NonQuery("SP_MUTATE_ShoppingCartItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@SCI_SHOPPING_CART", SqlDbType.UniqueIdentifier) { Value = shoppingCartItem.ShoppingCartId },
                new SqlParameter("@SCI_PRODUCT", SqlDbType.UniqueIdentifier) { Value = shoppingCartItem.ProductId },
                new SqlParameter("@SCI_AMOUNT", SqlDbType.Int) { Value = shoppingCartItem.Amount }
            ]);

        public async Task<MutationResult> CreateTransaction(Transaction transaction, Guid mutationId)
            => await NonQuery("SP_MUTATE_Transaction", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@TRX_ID", SqlDbType.UniqueIdentifier) { Value = transaction.Id },
                new SqlParameter("@TRX_SHOP", SqlDbType.UniqueIdentifier) { Value = transaction.ShopId },
                new SqlParameter("@TRX_RECIPIENT", SqlDbType.VarChar) { Value = transaction.Recipient },
                new SqlParameter("@TRX_AMOUNT_DUE", SqlDbType.Decimal) { Value = transaction.AmountDue },
                new SqlParameter("@TRX_AMOUNT_PAID", SqlDbType.Decimal) { Value = transaction.AmountPaid },
                new SqlParameter("@TRX_TX", SqlDbType.VarChar) { Value = transaction.Tx }
            ], mutationId);
        #endregion

        #region Delete
        public async Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId }
            ], mutationId);

        public async Task<MutationResult> DeleteCustomer(Guid customerId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Customer", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = customerId }
            ], mutationId);

        public async Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = deliveryMethodId }
            ], mutationId);

        public async Task<MutationResult> DeleteDeliveryMethodCostsPerCountry(Guid deliveryMethodId, Guid countryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethodCostsPerCountry", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@CPC_DELIVERY_METHOD", SqlDbType.UniqueIdentifier) { Value = deliveryMethodId },
                new SqlParameter("@CPC_COUNTRY", SqlDbType.UniqueIdentifier) { Value = countryId }
            ], mutationId);

        public async Task<MutationResult> DeleteDigiByteWallet(Guid digiByteWalletId, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = digiByteWalletId }
            ], mutationId);

        public async Task<MutationResult> DeleteOrder(Guid orderId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Order", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@ORD_ID", SqlDbType.UniqueIdentifier) { Value = orderId }
            ], mutationId);

        public async Task<MutationResult> DeleteOrderItem(Guid orderItemId, Guid mutationId)
            => await NonQuery("SP_MUTATE_OrderItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@ORI_ID", SqlDbType.UniqueIdentifier) { Value = orderItemId }
            ], mutationId);

        public async Task<MutationResult> DeletePage(Guid pageId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Page", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@PAG_ID", SqlDbType.UniqueIdentifier) { Value = pageId }
            ], mutationId);

        public async Task<MutationResult> DeletePage2Category(Guid pageId, Guid categoryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Page2Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@P2C_PAGE", SqlDbType.UniqueIdentifier) { Value = pageId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = categoryId }
            ], mutationId);

        public async Task<MutationResult> DeleteProduct(Guid productId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = productId }
            ], mutationId);

        public async Task<MutationResult> DeleteProduct2Category(Guid productId, Guid categoryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product2Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = categoryId }
            ], mutationId);

        public async Task<MutationResult> DeleteProductPhoto(Guid productPhotoId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId }
            ], mutationId);

        public async Task<MutationResult> DeleteShop(Guid shopId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = shopId }
            ], mutationId);

        public async Task<MutationResult> DeleteShoppingCartItem(Guid shoppingCartItemId)
            => await NonQuery("SP_MUTATE_ShoppingCartItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@SCI_ID", SqlDbType.UniqueIdentifier) { Value = shoppingCartItemId }
            ]);
        #endregion

        #region Get
        public async Task<DataTable> GetAddress(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryIds)
            => await Get("SP_GET_Address", [
                new SqlParameter("@ADR_ADDRESS_LINE_1", SqlDbType.NVarChar) { Value = addressLine1 },
                new SqlParameter("@ADR_ADDRESS_LINE_2", SqlDbType.NVarChar) { Value = addressLine2 },
                new SqlParameter("@ADR_POSTAL_CODE", SqlDbType.NVarChar) { Value = postalCode },
                new SqlParameter("@ADR_CITY", SqlDbType.NVarChar) { Value = city },
                new SqlParameter("@ADR_PROVINCE", SqlDbType.NVarChar) { Value = province },
                new SqlParameter("@ADR_COUNTRY", SqlDbType.UniqueIdentifier) { Value = countryIds }
            ]);

        public async Task<DataTable> GetCategories(GetCategoriesParameters parameters)
            => await Get("SP_GET_Categories", [
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@CAT_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parameters.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@CAT_VISIBLE", SqlDbType.Bit) { Value = parameters.Visible }
            ]);

        public async Task<DataTable> GetCountries(GetCountriesParameters parameters)
            => await Get("SP_GET_Countries", [
                new SqlParameter("@CTR_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CTR_CODE", SqlDbType.Char) { Value = parameters.Code },
                new SqlParameter("@CTR_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetCurrencies(GetCurrenciesParameters parameters)
            => await Get("SP_GET_Currencies", [
                new SqlParameter("@CUR_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CUR_SYMBOL", SqlDbType.NChar) { Value = parameters.Symbol },
                new SqlParameter("@CUR_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetCustomers(GetCustomersParameters parameters)
            => await Get("SP_GET_Customers", [
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CUS_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@CUS_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@CUS_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress },
                new SqlParameter("@CUS_USERNAME", SqlDbType.VarChar) { Value = parameters.Username },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar) { Value = parameters.Password },
                new SqlParameter("@CUS_FIRST_NAME", SqlDbType.NVarChar) { Value = parameters.FirstName },
                new SqlParameter("@CUS_LAST_NAME", SqlDbType.NVarChar) { Value = parameters.LastName }
            ]);

        public async Task<DataTable> GetCustomerByEmailAddress(Guid shopId, string emailAddress)
           => await Get("SP_GET_Customer_ByEmailAddress", [
               new SqlParameter("@CUS_SHOP", SqlDbType.UniqueIdentifier) { Value = shopId },
               new SqlParameter("@CUS_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress }
           ]);

        public async Task<DataTable> GetCustomerByEmailAddressAndPassword(Guid shopId, string emailAddress, string password, string? ipAddress)
            => await Get("SP_GET_Customer_ByEmailAddressAndPassword", [
                new SqlParameter("@CUS_SHOP", SqlDbType.UniqueIdentifier) { Value = shopId },
                new SqlParameter("@CUS_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar) { Value = password },
                new SqlParameter("@CUS_IP_ADDRESS", SqlDbType.VarChar) { Value = ipAddress }
            ]);

        public async Task<DataTable> GetCustomerByIdAndPassword(Guid id, string password)
            => await Get("SP_GET_Customer_ByIdAndPassword", [
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar) { Value = password }
            ]);

        public async Task<DataTable> GetDashboardSales(Guid merchantId, string mode)
            => await Get("SP_GET_DashboardSales", [
                new SqlParameter("@MERCHANT_ID", SqlDbType.UniqueIdentifier) { Value = merchantId },
                new SqlParameter("@MODE", SqlDbType.VarChar) { Value = mode }
            ]);

        public async Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters)
            => await Get("SP_GET_DeliveryMethods", [
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@DLM_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetDeliveryMethodCostsPerCountry(GetDeliveryMethodCostsPerCountryParameters parameters)
            => await Get("SP_GET_DeliveryMethodCostsPerCountry", [
                new SqlParameter("@CPC_DELIVERY_METHOD", SqlDbType.UniqueIdentifier) { Value = parameters.DeliveryMethodId },
                new SqlParameter("@CPC_COUNTRY", SqlDbType.UniqueIdentifier) { Value = parameters.CountryId }
            ]);

        public async Task<DataTable> GetDigiByteWallets(GetDigiByteWalletsParameters parameters)
            => await Get("SP_GET_DigiByteWallets", [
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar) { Value = parameters.Address }
            ]);

        public async Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters)
            => await Get("SP_GET_FaqCategories", [
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetFaqs(GetFaqsParameters parameters)
            => await Get("SP_GET_Faqs", [
                new SqlParameter("@FAQ_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@FAQ_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("@FAQ_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@FAQ_KEYWORDS", SqlDbType.NVarChar) { Value = parameters.Keywords },
            ]);

        public async Task<DataTable> GetFinancialStatementTransactions(GetFinancialStatementTransactionsParameters parameters)
            => await Get("SP_GET_FinancialStatementTransactions", [
                new SqlParameter("@TRX_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@TRX_DATE_FROM", SqlDbType.Date) { Value = parameters.DateFrom },
                new SqlParameter("@TRX_DATE_UNTIL", SqlDbType.Date) { Value = parameters.DateUntil },
                new SqlParameter("@TRX_TYPE", SqlDbType.TinyInt) { Value = parameters.Type },
                new SqlParameter("@TRX_CURRENCY", SqlDbType.UniqueIdentifier) { Value = parameters.CurrencyId },
                new SqlParameter("@TRX_RECURRANCE", SqlDbType.TinyInt) { Value = parameters.Recurrance },
                new SqlParameter("@TRX_DESCRIPTION", SqlDbType.NVarChar) { Value = parameters.Description },
            ]);

        public async Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters)
            => await Get("SP_GET_NewsMessages", [
                new SqlParameter("@NWS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@NWS_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@NWS_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("@NWS_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil }
            ]);

        public async Task<DataTable> GetMerchantByEmailAddress(string emailAddress)
            => await Get("SP_GET_Merchant_ByEmailAddress", [
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress }
            ]);

        public async Task<DataTable> GetMerchantByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
            => await Get("SP_GET_Merchant_ByEmailAddressAndPassword", [
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = password },
                new SqlParameter("@MER_IP_ADDRESS", SqlDbType.VarChar) { Value = ipAddress }
            ]);

        public async Task<DataTable> GetMerchantByIdAndPassword(Guid id, string password)
            => await Get("SP_GET_Merchant_ByIdAndPassword", [
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = password }
            ]);

        public async Task<DataTable> GetMerchantPasswordResetLinkByIdAndKey(Guid id, string key)
            => await Get("SP_GET_MerchantPasswordResetLink_ByIdAndKey", [
                new SqlParameter("@PRL_ID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@PRL_KEY", SqlDbType.VarChar) { Value = key },
            ]);

        public async Task<DataTable> GetMerchants(GetMerchantsParameters parameters)
            => await Get("SP_GET_Merchants", [
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress },
                new SqlParameter("@MER_USERNAME", SqlDbType.VarChar) { Value = parameters.Username },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = parameters.Password },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar) { Value = parameters.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar) { Value = parameters.LastName }
            ]);

        public async Task<DataTable> GetOrders(GetOrdersParameters parameters)
            => await Get("SP_GET_Orders", [
                new SqlParameter("@ORD_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@ORD_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@ORD_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@ORD_CUSTOMER", SqlDbType.UniqueIdentifier) { Value = parameters.CustomerId },
                new SqlParameter("@ORD_CUSTOMER_CUSTOM", SqlDbType.NVarChar) { Value = parameters.Customer },
                new SqlParameter("@ORD_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("@ORD_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil },
                new SqlParameter("@ORD_STATUS", SqlDbType.TinyInt) { Value = parameters.Status }
            ]);

        public async Task<DataTable> GetOrderItems(GetOrderItemsParameters parameters)
            => await Get("SP_GET_OrderItems", [
                new SqlParameter("@ORI_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@ORI_ORDER", SqlDbType.UniqueIdentifier) { Value = parameters.OrderId },
                new SqlParameter("@ORI_TYPE", SqlDbType.TinyInt) { Value = parameters.Type },
                new SqlParameter("@ORI_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.ProductId },
                new SqlParameter("@ORI_DESCRIPTION", SqlDbType.NVarChar) { Value = parameters.Description }
            ]);

        public async Task<DataTable> GetPages(GetPagesParameters parameters)
            => await Get("SP_GET_Pages", [
                new SqlParameter("@PAG_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@PAG_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@PAG_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@PAG_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@PAG_VISIBLE", SqlDbType.Bit) { Value = parameters.Visible }
            ]);

        public async Task<DataTable> GetPage2Categories(GetPage2CategoriesParameters parameters)
            => await Get("SP_GET_Page2Categories", [
                new SqlParameter("@P2C_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.PageId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId }
            ]);

        public async Task<DataTable> GetPageCategories(GetPageCategoriesParameters parameters)
            => await Get("SP_GET_PageCategories", [
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetProducts(GetProductsParameters parameters)
            => await Get("SP_GET_Products", [
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@PRD_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@PRD_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("@PRD_CODE", SqlDbType.NVarChar) { Value = parameters.Code },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@PRD_VISIBLE", SqlDbType.Bit) { Value = parameters.Visible },
                new SqlParameter("@PRD_SHOW_ON_HOME", SqlDbType.Bit) { Value = parameters.ShowOnHome }
            ]);

        public async Task<DataTable> GetProduct2Categories(GetProduct2CategoriesParameters parameters)
            => await Get("SP_GET_Product2Categories", [
                new SqlParameter("@P2C_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.ProductId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId }
            ]);

        public async Task<DataTable> GetProductPhotos(GetProductPhotosParameters parameters)
            => await Get("SP_GET_ProductPhotos", [
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@PHT_PRODUCT_SHOP_MERCHANT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.ProductId }
            ]);

        public async Task<DataTable> GetShopCategories(GetShopCategoriesParameters parameters)
            => await Get("SP_GET_ShopCategories", [
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            ]);

        public async Task<DataTable> GetShops(GetShopsParameters parameters)
            => await Get("SP_GET_Shops", [
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.NVarChar) { Value = parameters.SubDomain },
                new SqlParameter("@SHP_COUNTRY", SqlDbType.UniqueIdentifier) { Value = parameters.CountryId },
                new SqlParameter("@SHP_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("@SHP_FEATURED", SqlDbType.Bit) { Value = parameters.Featured },
                new SqlParameter("@SHP_USABLE", SqlDbType.Bit) { Value = parameters.Usable }
            ]);

        public async Task<DataTable> GetShopByIdAndSubDomain(Guid? id, string subDomain)
            => await Get("SP_GET_Shop_BySubDomain", [
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.NVarChar) { Value = subDomain }
            ]);

        public async Task<DataTable> GetShoppingCarts(GetShoppingCartsParameters parameters)
            => await Get("SP_GET_ShoppingCarts", [
                new SqlParameter("@SHC_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@SHC_SESSION", SqlDbType.UniqueIdentifier) { Value = parameters.Session },
                new SqlParameter("@SHC_CUSTOMER", SqlDbType.UniqueIdentifier) { Value = parameters.CustomerId }
            ]);

        public async Task<DataTable> GetShoppingCartItems(GetShoppingCartItemsParameters parameters)
            => await Get("SP_GET_ShoppingCartItems", [
                new SqlParameter("@SCI_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@SCI_SHOPPING_CART", SqlDbType.UniqueIdentifier) { Value = parameters.ShoppingCartId },
                new SqlParameter("@SCI_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.ProductId }
            ]);

        public async Task<DataTable> GetStats()
            => await Get("SP_GET_Stats", []);

        public async Task<DataTable> GetTransactions(GetTransactionsParameters parameters)
            => await Get("SP_GET_TRANSACTIONS", [
                new SqlParameter("@TRX_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@TRX_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@TRX_RECIPIENT", SqlDbType.VarChar) { Value = parameters.Recipient },
                new SqlParameter("@TRX_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("@TRX_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil },
                new SqlParameter("@TRX_UNPAID", SqlDbType.Bit) { Value = parameters.Unpaid }
            ]);
        #endregion

        #region Update
        public async Task<MutationResult> UpdateCategory(Category category, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = category.Id },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.ShopId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_VISIBLE", SqlDbType.Bit) { Value = category.Visible }
            ], mutationId);

        public async Task<MutationResult> UpdateCategoryChangeParent(Guid categoryId, Guid parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            ], mutationId);

        public async Task<MutationResult> UpdateCategoryMoveDown(Guid categoryId, Guid? parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            ], mutationId);

        public async Task<MutationResult> UpdateCategoryMoveUp(Guid categoryId, Guid? parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 23 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            ], mutationId);

        public async Task<MutationResult> UpdateCustomer(Customer customer, Guid mutationId)
            => await NonQuery("SP_MUTATE_Customer", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = customer.Id },
                new SqlParameter("@CUS_SHOP", SqlDbType.UniqueIdentifier) { Value = customer.ShopId },
                new SqlParameter("@CUS_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = customer.EmailAddress },
                new SqlParameter("@CUS_USERNAME", SqlDbType.VarChar) { Value = customer.Username },
                new SqlParameter("@CUS_GENDER", SqlDbType.TinyInt) { Value = customer.Gender },
                new SqlParameter("@CUS_FIRST_NAME", SqlDbType.NVarChar, 255) { Value = customer.FirstName },
                new SqlParameter("@CUS_LAST_NAME", SqlDbType.NVarChar, 255) { Value = customer.LastName },
                new SqlParameter("@CUS_ADDRESS", SqlDbType.UniqueIdentifier) { Value = customer.Address!.Id }
            ], mutationId);

        public async Task<MutationResult> UpdateCustomerPasswordAndActivate(Customer customer, string password, Guid mutationId)
            => await NonQuery("SP_MUTATE_Customer", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = customer.Id },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar, 100) { Value = password }
            ], mutationId);

        public async Task<MutationResult> UpdateCustomerPasswordAndSalt(Customer customer, string password, string passwordSalt, Guid mutationId)
            => await NonQuery("SP_MUTATE_Customer", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@CUS_ID", SqlDbType.UniqueIdentifier) { Value = customer.Id },
                new SqlParameter("@CUS_PASSWORD", SqlDbType.VarChar, 100) { Value = password },
                new SqlParameter("@CUS_PASSWORD_SALT", SqlDbType.VarChar, 24) { Value = passwordSalt }
            ], mutationId);

        public async Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Id },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Shop.Id },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar, 255) { Value = deliveryMethod.Name },
                new SqlParameter("@DLM_COSTS", SqlDbType.Decimal) { Value = deliveryMethod.Costs }
            ], mutationId);

        public async Task<MutationResult> UpdateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.Id },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.MerchantId },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar, 255) { Value = digiByteWallet.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar, 100) { Value = digiByteWallet.Address }
            ], mutationId);

        public async Task<MutationResult> UpdateMerchant(Merchant merchant, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = merchant.Id },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar, 255) { Value = merchant.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar, 255) { Value = merchant.LastName }
            ], mutationId);

        public async Task<MutationResult> UpdateMerchantPasswordAndActivate(Merchant merchant, string password, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = merchant.Id },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar, 100) { Value = password }
            ], mutationId);

        public async Task<MutationResult> UpdateMerchantPasswordAndSalt(Merchant merchant, string password, string passwordSalt, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = merchant.Id },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar, 100) { Value = password },
                new SqlParameter("@MER_PASSWORD_SALT", SqlDbType.VarChar, 24) { Value = passwordSalt }
            ], mutationId);

        public async Task<MutationResult> UpdateMerchantPasswordResetLinkUsed(MerchantPasswordResetLink merchantPasswordResetLink, Guid mutationId)
            => await NonQuery("SP_MUTATE_MerchantPasswordResetLink", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@PRL_ID", SqlDbType.UniqueIdentifier) { Value = merchantPasswordResetLink.Id }
            ], mutationId);

        public async Task<MutationResult> UpdateOrder(Order order, Guid mutationId)
            => await NonQuery("SP_MUTATE_Order", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@ORD_ADDRESS_BILLING", SqlDbType.UniqueIdentifier) { Value = order.BillingAddress.Id },
                new SqlParameter("@ORD_ADDRESS_SHIPPING", SqlDbType.UniqueIdentifier) { Value = order.ShippingAddress.Id },
                new SqlParameter("@ORD_DELIVERY_METHOD", SqlDbType.UniqueIdentifier) { Value = order.DeliveryMethodId },
                new SqlParameter("@ORD_COMMENTS", SqlDbType.NVarChar) { Value = order.Comments },
                new SqlParameter("@ORD_SENDER_WALLET_ADDRESS", SqlDbType.VarChar) { Value = order.SenderWalletAddress }
            ], mutationId);

        public async Task<MutationResult> UpdateOrderStatus(Guid orderId, OrderStatus status, Guid mutationId)
            => await NonQuery("SP_MUTATE_Order", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@ORD_ID", SqlDbType.UniqueIdentifier) { Value = orderId },
                new SqlParameter("@ORD_STATUS", SqlDbType.TinyInt) { Value = status }
            ], mutationId);

        public async Task<MutationResult> UpdateOrderTransaction(Guid orderId, Guid transactionId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Order", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@ORD_ID", SqlDbType.UniqueIdentifier) { Value = orderId },
                new SqlParameter("@ORD_TRANSACTION", SqlDbType.UniqueIdentifier) { Value = transactionId }
            ], mutationId);

        public async Task<MutationResult> UpdateOrderItem(OrderItem orderItem, Guid mutationId)
            => await NonQuery("SP_MUTATE_OrderItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@ORI_ID", SqlDbType.UniqueIdentifier) { Value = orderItem.Id },
                new SqlParameter("@ORI_AMOUNT", SqlDbType.Int) { Value = orderItem.Amount },
                new SqlParameter("@ORI_PRICE", SqlDbType.Decimal) { Value = orderItem.Price },
                new SqlParameter("@ORI_DESCRIPTION", SqlDbType.NVarChar) { Value = orderItem.Description }
            ], mutationId);

        public async Task<MutationResult> UpdatePage(Page page, Guid mutationId)
            => await NonQuery("SP_MUTATE_Page", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@PAG_ID", SqlDbType.UniqueIdentifier) { Value = page.Id },
                new SqlParameter("@PAG_SHOP", SqlDbType.UniqueIdentifier) { Value = page.Shop.Id },
                new SqlParameter("@PAG_TITLE", SqlDbType.NVarChar) { Value = page.Title },
                new SqlParameter("@PAG_CONTENT", SqlDbType.NVarChar) { Value = page.Content },
                new SqlParameter("@PAG_VISIBLE", SqlDbType.Bit) { Value = page.Visible },
                new SqlParameter("@PAG_INDEX", SqlDbType.Bit) { Value = page.Index }
            ], mutationId);

        public async Task<MutationResult> UpdateProduct(Product product, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = product.Id },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = product.ShopId },
                new SqlParameter("@PRD_CODE", SqlDbType.NVarChar) { Value = product.Code },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = product.Name },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = product.Description },
                new SqlParameter("@PRD_STOCK", SqlDbType.Int) { Value = product.Stock },
                new SqlParameter("@PRD_PRICE", SqlDbType.Decimal) { Value = product.Price },
                new SqlParameter("@PRD_VISIBLE", SqlDbType.Bit) { Value = product.Visible },
                new SqlParameter("@PRD_SHOW_ON_HOME", SqlDbType.Bit) { Value = product.ShowOnHome }
            ], mutationId);

        public async Task<MutationResult> UpdateProductDuplicate(Guid productId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = productId }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhoto(ProductPhoto productPhoto, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhoto.Id },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = productPhoto.Description },
                new SqlParameter("@PHT_SORTORDER", SqlDbType.Int) { Value = productPhoto.SortOrder },
                new SqlParameter("@PHT_MAIN", SqlDbType.Bit) { Value = productPhoto.Main },
                new SqlParameter("@PHT_VISIBLE", SqlDbType.Bit) { Value = productPhoto.Visible }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhotoChangeDescription(Guid productPhotoId, string description, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId },
                new SqlParameter("@PHT_DESCRIPTION", SqlDbType.NVarChar) { Value = description }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhotoChangeMain(Guid productPhotoId, Guid productId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productId }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhotoChangeVisible(Guid productPhotoId, bool visible, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 23 },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId },
                new SqlParameter("@PHT_VISIBLE", SqlDbType.NVarChar) { Value = visible }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhotoMoveDown(Guid productPhotoId, Guid productId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 24 },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productId }
            ], mutationId);

        public async Task<MutationResult> UpdateProductPhotoMoveUp(Guid productPhotoId, Guid productId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 25 },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productId }
            ], mutationId);

        public async Task<MutationResult> UpdateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = shop.Id },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar, 255) { Value = shop.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.VarChar, 100) { Value = shop.SubDomain },
                new SqlParameter("@SHP_COUNTRY", SqlDbType.UniqueIdentifier) { Value = shop.Country?.Id },
                new SqlParameter("@SHP_CATEGORY", SqlDbType.UniqueIdentifier) { Value = shop.Category?.Id },
                new SqlParameter("@SHP_WALLET", SqlDbType.UniqueIdentifier) { Value = shop.Wallet?.Id },
                new SqlParameter("@SHP_ORDER_METHOD", SqlDbType.TinyInt) { Value = shop.OrderMethod },
                new SqlParameter("@SHP_REQUIRE_ADDRESSES", SqlDbType.Bit) { Value = shop.RequireAddresses }
            ], mutationId);

        public async Task<MutationResult> UpdateShoppingCartItem(ShoppingCartItem shoppingCartItem)
            => await NonQuery("SP_MUTATE_ShoppingCartItem", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@SCI_ID", SqlDbType.UniqueIdentifier) { Value = shoppingCartItem.Id },
                new SqlParameter("@SCI_AMOUNT", SqlDbType.Int) { Value = shoppingCartItem.Amount }
            ]);

        public async Task<MutationResult> UpdateTransactionAmountPaid(Guid transactionId, decimal amountPaid, Guid mutationId)
            => await NonQuery("SP_MUTATE_Transaction", [
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@TRX_ID", SqlDbType.UniqueIdentifier) { Value = transactionId },
                new SqlParameter("@TRX_AMOUNT_PAID", SqlDbType.Decimal) { Value = amountPaid }
            ], mutationId);
        #endregion

        private async Task<DataTable> Get(string storedProcedure, List<SqlParameter> parameters)
        {
            DataTable table = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

                using SqlDataAdapter adapter = new(command);
                await Task.Run(() => adapter.Fill(table));
            }

            return table;
        }

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters)
            => await NonQuery(storedProcedure, parameters, null);

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters, Guid? mutationId)
        {
            MutationResult result = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };

                if (mutationId.HasValue)
                    command.Parameters.Add(new SqlParameter("@MUT_ID", SqlDbType.UniqueIdentifier) { Value = mutationId });

                command.Parameters.Add(new SqlParameter("@OUT_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@OUT_IDENTITY", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@OUT_MESSAGE", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });

                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

                connection.Open();

                try
                {
                    await command.ExecuteNonQueryAsync();
                    result.ErrorCode = int.TryParse(command.Parameters["@OUT_ERROR"].Value.ToString(), out var outErrorCode) ? outErrorCode : 0;
                    result.Identifier = Guid.TryParse(command.Parameters["@OUT_IDENTITY"].Value.ToString(), out var outIdentity) ? outIdentity : Guid.Empty;
                    result.Message = command.Parameters["@OUT_MESSAGE"].Value.ToString();
                }
                catch
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}