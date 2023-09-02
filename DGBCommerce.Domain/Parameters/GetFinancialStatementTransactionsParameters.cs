using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Parameters
{
    public class GetFinancialStatementTransactionsParameters : GetParameters
    {
        public Guid? Id { get; set; }        
        public FinancialStatementTransactionType? Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
        public Guid? CurrencyId { get; set; }
        public Recurrance? Recurrance { get; set; }
        public string? Description { get; set; }
    }
}