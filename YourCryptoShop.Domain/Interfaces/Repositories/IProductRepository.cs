﻿using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;

namespace YourCryptoShop.Domain.Interfaces.Repositories
{
    public interface IProductRepository : IMutableRepository<Product, GetProductsParameters> 
    {
        Task<IEnumerable<PublicProduct>> GetPublic(GetProductsParameters parameters);
        Task<PublicProduct?> GetByIdPublic(Guid shopId, Guid id);
        Task<MutationResult> Duplicate(Guid id, Guid mutationId);
    }
}