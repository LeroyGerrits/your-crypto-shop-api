﻿namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicProduct
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public decimal Price { get; set; }
        public Guid? MainPhotoId { get; set; }
        public string? MainPhotoExtension { get; set; }
        public List<PublicProductPhoto>? Photos { get; set; }
        public Dictionary<Guid, string>? FieldData { get; set; }
    }
}