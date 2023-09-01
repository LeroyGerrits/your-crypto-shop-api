﻿using DGBCommerce.Domain.Models;
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
        Task<MutationResult> CreateShop(Shop shop, Guid mutationId);

        Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId);
        Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId);
        Task<MutationResult> DeleteDigiByteWallet(Guid digiByteWalletId, Guid mutationId);
        Task<MutationResult> DeleteShop(Guid shopId, Guid mutationId);

        Task<DataTable> GetCategories(GetCategoriesParameters parameters);
        Task<DataTable> GetCurrencies(GetCurrenciesParameters parameters);
        Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters);
        Task<DataTable> GetDigiByteWallets(GetDigiByteWalletsParameters parameters);
        Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters);
        Task<DataTable> GetFaqs(GetFaqsParameters parameters);
        Task<DataTable> GetFinancialStatementTransactions(GetFinancialStatementTransactionsParameters parameters);
        Task<DataTable> GetMerchantByEmailAddress(string emailAddress);
        Task<DataTable> GetMerchantByEmailAddressAndPassword(string emailAddress, string password);
        Task<DataTable> GetMerchantPasswordResetLinkByIdAndKey(Guid id, string key);
        Task<DataTable> GetMerchants(GetMerchantsParameters parameters);
        Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters);
        Task<DataTable> GetShops(GetShopsParameters parameters);
        Task<DataTable> GetShopBySubDomain(string subDomain);

        Task<MutationResult> UpdateCategory(Category category, Guid mutationId);
        Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId);
        Task<MutationResult> UpdateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId);
        Task<MutationResult> UpdateMerchant(Merchant merchant, Guid mutationId);
        Task<MutationResult> UpdateShop(Shop shop, Guid mutationId);
    }
}