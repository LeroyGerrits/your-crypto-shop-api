using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class CustomerRepository(IDataAccessLayer dataAccessLayer) : ICustomerRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Customer>> Get(GetCustomersParameters parameters)
            => await GetRaw(parameters);

        public async Task<Customer?> GetById(Guid merchantId, Guid id)
        {
            var customers = await GetRaw(new GetCustomersParameters() { MerchantId = merchantId, Id = id });
            return customers.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicCustomer>> GetPublic(GetCustomersParameters parameters)
            => await GetRawPublic(parameters);

        public async Task<PublicCustomer?> GetByIdPublic(Guid merchantId, Guid id)
        {
            var customers = await GetRawPublic(new GetCustomersParameters() { MerchantId = merchantId, Id = id });
            return customers.ToList().SingleOrDefault();
        }

        public async Task<Customer?> GetByEmailAddress(string emailAddress)
            => await GetRawByEmailAddress(emailAddress);

        public async Task<Customer?> GetByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
            => await GetRawByEmailAddressAndPassword(emailAddress, password, ipAddress);

        public async Task<Customer?> GetByIdAndPassword(Guid id, string password)
            => await GetRawByIdAndPassword(id, password);

        public Task<MutationResult> Create(Customer item, Guid mutationId)
            => _dataAccessLayer.CreateCustomer(item, mutationId);

        public Task<MutationResult> Update(Customer item, Guid mutationId)
            => _dataAccessLayer.UpdateCustomer(item, mutationId);

        public Task<MutationResult> UpdatePasswordAndSalt(Customer item, string password, string passwordSalt, Guid mutationId)
            => _dataAccessLayer.UpdateCustomerPasswordAndSalt(item, password, passwordSalt, mutationId);

        public Task<MutationResult> UpdatePasswordAndActivate(Customer item, string password, Guid mutationId)
            => _dataAccessLayer.UpdateCustomerPasswordAndActivate(item, password, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => throw new NotImplementedException();

        private async Task<IEnumerable<Customer>> GetRaw(GetCustomersParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCustomers(parameters);
            List<Customer> customers = [];

            foreach (DataRow row in table.Rows)
            {
                customers.Add(new Customer()
                {
                    Id = new Guid(row["cus_id"].ToString()!),
                    ShopId = new Guid(row["cus_shop"].ToString()!),
                    Activated = Utilities.DBNullableDateTime(row["cus_activated"]),
                    EmailAddress = Utilities.DbNullableString(row["cus_email_address"]),
                    Username = Utilities.DbNullableString(row["cus_username"]),
                    PasswordSalt = Utilities.DbNullableString(row["cus_password_salt"]),
                    Gender = (Gender)Convert.ToInt32(row["cus_gender"]),
                    FirstName = Utilities.DbNullableString(row["cus_first_name"]),
                    LastName = Utilities.DbNullableString(row["cus_last_name"]),
                    Address = new()
                    {
                        Id = new Guid(row["cus_address"].ToString()!),
                        AddressLine1 = Utilities.DbNullableString(row["cus_address_address_line_1"]),
                        AddressLine2 = Utilities.DbNullableString(row["cus_address_address_line_2"]),
                        PostalCode = Utilities.DbNullableString(row["cus_address_postal_code"]),
                        City = Utilities.DbNullableString(row["cus_address_city"]),
                        Province = Utilities.DbNullableString(row["cus_address_province"]),
                        Country = new()
                        {
                            Id = new Guid(row["cus_address_country"].ToString()!),
                            Code = Utilities.DbNullableString(row["cus_address_country_code"]),
                            Name = Utilities.DbNullableString(row["cus_address_country_name"])
                        }
                    },
                    LastLogin = Utilities.DBNullableDateTime(row["cus_last_login"]),
                    LastIpAddress = Utilities.DbNullableString(row["cus_last_ip_address"]),
                    SecondLastLogin = Utilities.DBNullableDateTime(row["cus_second_last_login"]),
                    SecondLastIpAddress = Utilities.DbNullableString(row["cus_second_last_ip_address"])
                });
            }

            return customers;
        }

        private async Task<Customer?> GetRawByEmailAddress(string emailAddress)
        {
            DataTable table = await _dataAccessLayer.GetCustomerByEmailAddress(emailAddress);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Customer()
            {
                Id = new Guid(row["cus_id"].ToString()!),
                ShopId = new Guid(row["cus_shop"].ToString()!),
                Activated = Utilities.DBNullableDateTime(row["cus_activated"]),
                EmailAddress = Utilities.DbNullableString(row["cus_email_address"]),
                Username = Utilities.DbNullableString(row["cus_username"]),
                PasswordSalt = Utilities.DbNullableString(row["cus_password_salt"]),
                Gender = (Gender)Convert.ToInt32(row["cus_gender"]),
                FirstName = Utilities.DbNullableString(row["cus_first_name"]),
                LastName = Utilities.DbNullableString(row["cus_last_name"]),
                Address = new()
                {
                    Id = new Guid(row["cus_address"].ToString()!),
                    AddressLine1 = Utilities.DbNullableString(row["cus_address_address_line_1"]),
                    AddressLine2 = Utilities.DbNullableString(row["cus_address_address_line_2"]),
                    PostalCode = Utilities.DbNullableString(row["cus_address_postal_code"]),
                    City = Utilities.DbNullableString(row["cus_address_city"]),
                    Province = Utilities.DbNullableString(row["cus_address_province"]),
                    Country = new()
                    {
                        Id = new Guid(row["cus_address_country"].ToString()!),
                        Code = Utilities.DbNullableString(row["cus_address_country_code"]),
                        Name = Utilities.DbNullableString(row["cus_address_country_name"])
                    }
                },
            };
        }

        private async Task<Customer?> GetRawByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
        {
            DataTable table = await _dataAccessLayer.GetCustomerByEmailAddressAndPassword(emailAddress, password, ipAddress);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Customer()
            {
                Id = new Guid(row["cus_id"].ToString()!),
                ShopId = new Guid(row["cus_shop"].ToString()!),
                Activated = Utilities.DBNullableDateTime(row["cus_activated"]),
                EmailAddress = Utilities.DbNullableString(row["cus_email_address"]),
                Username = Utilities.DbNullableString(row["cus_username"]),
                Gender = (Gender)Convert.ToInt32(row["cus_gender"]),
                FirstName = Utilities.DbNullableString(row["cus_first_name"]),
                LastName = Utilities.DbNullableString(row["cus_last_name"]),
                Address = new()
                {
                    Id = new Guid(row["cus_address"].ToString()!),
                    AddressLine1 = Utilities.DbNullableString(row["cus_address_address_line_1"]),
                    AddressLine2 = Utilities.DbNullableString(row["cus_address_address_line_2"]),
                    PostalCode = Utilities.DbNullableString(row["cus_address_postal_code"]),
                    City = Utilities.DbNullableString(row["cus_address_city"]),
                    Province = Utilities.DbNullableString(row["cus_address_province"]),
                    Country = new()
                    {
                        Id = new Guid(row["cus_address_country"].ToString()!),
                        Code = Utilities.DbNullableString(row["cus_address_country_code"]),
                        Name = Utilities.DbNullableString(row["cus_address_country_name"])
                    }
                },
                LastLogin = Utilities.DBNullableDateTime(row["cus_last_login"]),
                LastIpAddress = Utilities.DbNullableString(row["cus_last_ip_address"]),
                SecondLastLogin = Utilities.DBNullableDateTime(row["cus_second_last_login"]),
                SecondLastIpAddress = Utilities.DbNullableString(row["cus_second_last_ip_address"])
            };
        }

        private async Task<Customer?> GetRawByIdAndPassword(Guid id, string password)
        {
            DataTable table = await _dataAccessLayer.GetCustomerByIdAndPassword(id, password);

            if (table.Rows.Count != 1)
                return null;

            DataRow row = table.Rows[0];
            return new Customer()
            {
                Id = new Guid(row["cus_id"].ToString()!),
                ShopId = new Guid(row["cus_shop"].ToString()!),
                Activated = Utilities.DBNullableDateTime(row["cus_activated"]),
                EmailAddress = Utilities.DbNullableString(row["cus_email_address"]),
                Username = Utilities.DbNullableString(row["cus_username"]),
                Password = Utilities.DbNullableString(row["cus_password"]),
                PasswordSalt = Utilities.DbNullableString(row["cus_password_salt"]),
                Gender = (Gender)Convert.ToInt32(row["cus_gender"]),
                FirstName = Utilities.DbNullableString(row["cus_first_name"]),
                LastName = Utilities.DbNullableString(row["cus_last_name"]),
                Address = new()
                {
                    Id = new Guid(row["cus_address"].ToString()!),
                    AddressLine1 = Utilities.DbNullableString(row["cus_address_address_line_1"]),
                    AddressLine2 = Utilities.DbNullableString(row["cus_address_address_line_2"]),
                    PostalCode = Utilities.DbNullableString(row["cus_address_postal_code"]),
                    City = Utilities.DbNullableString(row["cus_address_city"]),
                    Province = Utilities.DbNullableString(row["cus_address_province"]),
                    Country = new()
                    {
                        Id = new Guid(row["cus_address_country"].ToString()!),
                        Code = Utilities.DbNullableString(row["cus_address_country_code"]),
                        Name = Utilities.DbNullableString(row["cus_address_country_name"])
                    }
                },
            };
        }

        private async Task<IEnumerable<PublicCustomer>> GetRawPublic(GetCustomersParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCustomers(parameters);
            List<PublicCustomer> customers = [];

            foreach (DataRow row in table.Rows)
            {
                customers.Add(new PublicCustomer()
                {
                    Id = new Guid(row["cus_id"].ToString()!),
                    Username = Utilities.DbNullableString(row["cus_username"])
                });
            }

            return customers;
        }
    }
}