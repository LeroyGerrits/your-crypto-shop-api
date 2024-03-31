﻿using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class DigiByteWalletRepository(IDataAccessLayer dataAccessLayer) : IDigiByteWalletRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<DigiByteWallet>> Get(GetDigiByteWalletsParameters parameters)
            => await GetRaw(parameters);

        public async Task<DigiByteWallet?> GetById(Guid merchantId, Guid id)
        {
            var deliveryMethods = await GetRaw(new GetDigiByteWalletsParameters() { MerchantId = merchantId, Id = id });
            return deliveryMethods.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(DigiByteWallet item, Guid mutationId)
            => _dataAccessLayer.CreateDigiByteWallet(item, mutationId);

        public Task<MutationResult> Update(DigiByteWallet item, Guid mutationId)
            => _dataAccessLayer.UpdateDigiByteWallet(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteDigiByteWallet(id, mutationId);

        private async Task<IEnumerable<DigiByteWallet>> GetRaw(GetDigiByteWalletsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetDigiByteWallets(parameters);
            List<DigiByteWallet> deliveryMethods = [];

            foreach (DataRow row in table.Rows)
            {
                deliveryMethods.Add(new()
                {
                    Id = new Guid(row["dbw_id"].ToString()!),
                    MerchantId = new Guid(row["dbw_merchant"].ToString()!),
                    Name = Utilities.DbNullableString(row["dbw_name"]),
                    Address = Utilities.DbNullableString(row["dbw_address"])
                });
            }

            return deliveryMethods;
        }
    }
}