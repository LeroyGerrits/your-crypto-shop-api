using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class FinancialStatementTransaction
    {
        public required Guid Id { get; set; }
        public required FinancialStatementTransactionType Type { get; set; }
        public required DateTime Date { get; set; }
        public required Currency Currency { get; set; }
        public required decimal Amount { get; set; }
        public required Recurrance Recurrance { get; set; }
        public required string Description { get; set; }
    }
}