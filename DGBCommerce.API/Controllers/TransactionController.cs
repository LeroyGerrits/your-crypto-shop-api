using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils,
        ITransactionRepository transactionRepository) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get(Guid? shopId, string? recipient, DateTime? dateFrom, DateTime? dateUntil, string? tx, bool? unpaid)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var Transactions = await transactionRepository.Get(new GetTransactionsParameters()
            {
                MerchantId = authenticatedMerchantId,
                ShopId = shopId,
                Recipient = recipient,
                DateFrom = dateFrom,
                DateUntil = dateUntil,
                Tx = tx,
                Unpaid = unpaid
            });
            return Ok(Transactions.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> Get(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var Transaction = await transactionRepository.GetById(authenticatedMerchantId.Value, id);
            if (Transaction == null)
                return NotFound();

            return Ok(Transaction);
        }
    }
}