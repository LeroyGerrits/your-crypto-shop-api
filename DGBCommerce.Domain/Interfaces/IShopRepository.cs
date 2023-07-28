using DGBCommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGBCommerce.Domain.Interfaces
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> Get();
        Task<Shop> GetById(Guid id);
        Task<MutationResult> Insert(Shop shop);
        Task<MutationResult> Update(Shop shop);
        Task<MutationResult> Delete(Guid id);
    }
}
