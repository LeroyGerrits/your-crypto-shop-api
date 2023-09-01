namespace DGBCommerce.Domain.Parameters
{
    public class GetFaqsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Title { get; set; }
        public string? Keywords { get; set; }
    }
}