﻿using YourCryptoShop.Domain.Models.Response.CryptoCompare;

namespace YourCryptoShop.Domain.Interfaces.Services
{
    public interface ICryptoCompareService
    {
        Task<GetRatesResponse> GetRates(List<string> from, List<string> to);
    }
}