using System.Data;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models;

namespace DGBCommerce.Data.Services
{
    public class AddressService(IDataAccessLayer dataAccessLayer) : IAddressService
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<Address?> GetAddress(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryId)
        {
            var addresses = await GetRaw(addressLine1, addressLine2, postalCode, city, province, countryId);
            return addresses.SingleOrDefault();
        }

        private async Task<IEnumerable<Address>> GetRaw(string addressLine1, string? addressLine2, string postalCode, string city, string? province, Guid countryId)
        {
            DataTable table = await _dataAccessLayer.GetAddress(addressLine1, addressLine2, postalCode, city, province, countryId);
            List<Address> addresses = [];

            foreach (DataRow row in table.Rows)
            {
                addresses.Add(new()
                {
                    Id = new Guid(row["adr_id"].ToString()!),
                    AddressLine1 = Utilities.DbNullableString(row["adr_address_line_1"]),
                    AddressLine2 = Utilities.DbNullableString(row["adr_address_line_2"]),
                    PostalCode = Utilities.DbNullableString(row["adr_postal_code"]),
                    City = Utilities.DbNullableString(row["adr_city"]),
                    Province = Utilities.DbNullableString(row["adr_country"]),
                    Country = new()
                    {
                        Id = new Guid(row["adr_country"].ToString()!),
                        Code = Utilities.DbNullableString(row["adr_country_code"]),
                        Name = Utilities.DbNullableString(row["adr_country_name"])
                    }
                });
            }

            return addresses;
        }
    }
}