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
    public class FinancialStatementTransactionController : ControllerBase
    {
        private readonly IFinancialStatementTransactionRepository _financialStatementTransactionRepository;

        public FinancialStatementTransactionController(IFinancialStatementTransactionRepository financialStatementTransactionRepository)
        {
            _financialStatementTransactionRepository = financialStatementTransactionRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialStatementTransaction>>> Get(FinancialStatementTransactionType? type, DateTime? dateFrom, DateTime? dateUntil, Guid? currencyId, Recurrance? recurrance, string? description)
        {
            var financialStatementTransactions = await _financialStatementTransactionRepository.Get(new GetFinancialStatementTransactionsParameters()
            {
                Type = type,
                DateFrom = dateFrom,
                DateUntil = dateUntil,
                CurrencyId = currencyId,
                Recurrance = recurrance,
                Description = description
            });
            return Ok(financialStatementTransactions.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialStatementTransaction>> Get(Guid id)
        {
            var financialStatementTransaction = await _financialStatementTransactionRepository.GetById(id);
            if (financialStatementTransaction == null) 
                return NotFound();

            return Ok(financialStatementTransaction);
        }
    }
}