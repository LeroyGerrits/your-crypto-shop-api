using System.Text.RegularExpressions;

namespace DGBCommerce.Domain
{
    public class MutationResult
    {
        public Guid Identifier { get; set; }
        public int ErrorCode { get; set; }
        public string? Message { get; set; }

        public string Constraint
        {
            get
            {
                return ErrorCode switch
                {
                    2627 => new Regex(@"'(\w+)'").Matches(Message!)[0].Value,
                    _ => !string.IsNullOrWhiteSpace(Message) ? Message : string.Empty,
                };
            }
        }
    }
}
