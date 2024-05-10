using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class Field
    {
        public Guid? Id { get; set; }
        public required Shop Shop { get; set; }
        public required string Name { get; set; }
        public FieldEntity Entity { get; set; }
        public FieldType Type { get; set; }
        public bool UserDefinedMandatory { get; set; }        
        public FieldDataType DataType { get; set; }
        public string[]? Enumerations { get; set; }
        public int? SortOrder { get; set; }
        public required bool Visible { get; set; }
    }
}