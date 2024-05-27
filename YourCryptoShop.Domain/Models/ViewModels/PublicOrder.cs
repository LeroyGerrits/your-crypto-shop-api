using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicOrder
    {
        public Guid Id { get; set; }
        public required Guid ShopId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public required OrderStatus Status { get; set; }
        public required Address BillingAddress { get; set; }
        public required Address ShippingAddress { get; set; }
        public required Guid DeliveryMethodId { get; set; }
        public string? Comments { get; set; }
        public string? SenderWalletAddress { get; set; }
        public Guid? TransactionId { get; set; }
        public string? TransactionRecipient { get; set; }
        public decimal? TransactionAmountDue { get; set; }
        public decimal? TransactionAmountPaid { get; set; }
        public DateTime? TransactionPaidInFull { get; set; }
        public string? TransactionTx { get; set; }
    }
}