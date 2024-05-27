using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicField
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool UserDefinedMandatory { get; set; }
        public required FieldDataType DataType { get; set; }
        public string[]? Enumerations { get; set; }
    }
}