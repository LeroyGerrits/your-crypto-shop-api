namespace DGBCommerce.Domain.Parameters
{
    public class GetFaqsParameters
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Keywords { get; set; }
    }
}