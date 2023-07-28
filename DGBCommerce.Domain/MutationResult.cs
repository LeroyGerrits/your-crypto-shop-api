namespace DGBCommerce.Domain
{
    public class MutationResult
    {
        public Guid Identifier { get; set; }
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }

        public string Constraint
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ErrorMessage) ? ErrorMessage : string.Empty;
            }
        }
    }
}
