namespace DGBCommerce.Domain.Parameters
{
    public class GetNewsMessagesParameters
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
    }
}