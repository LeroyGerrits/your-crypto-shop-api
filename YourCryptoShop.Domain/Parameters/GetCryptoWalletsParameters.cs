﻿namespace YourCryptoShop.Domain.Parameters
{
    public class GetCryptoWalletsParameters : GetParameters
    {
        public Guid MerchantId { get; set; }
        public Guid? CurrencyId { get; set; }
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}