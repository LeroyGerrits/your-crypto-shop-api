namespace DGBCommerce.Domain.Parameters
{
    public abstract class GetParameters
    {
        public int PageSize { get; set; } = 25;
        public int CurrentPage { get; set; } = 0;
        public string? SortOrder { get; set; }
    }
}