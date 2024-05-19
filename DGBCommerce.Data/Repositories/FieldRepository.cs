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
    public class FieldRepository(IDataAccessLayer dataAccessLayer) : IFieldRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Field>> Get(GetFieldsParameters parameters)
            => await GetRaw(parameters);

        public async Task<Field?> GetById(Guid merchantId, Guid id)
        {
            var fields = await GetRaw(new GetFieldsParameters() { MerchantId = merchantId, Id = id });
            return fields.ToList().SingleOrDefault();
        }

        public async Task<PublicField?> GetByIdPublic(Guid id)
        {
            var categories = await GetRawPublic(new GetFieldsParameters() { Id = id });
            return categories.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicField>> GetByShopIdPublic(Guid shopId, FieldEntity? entity, FieldType? type)
        {
            var categories = await GetRawPublic(new GetFieldsParameters() { ShopId = shopId, Entity = entity, Type = type });
            return categories.ToList();
        }

        public Task<MutationResult> Create(Field item, Guid mutationId)
            => _dataAccessLayer.CreateField(item, mutationId);

        public Task<MutationResult> Update(Field item, Guid mutationId)
            => _dataAccessLayer.UpdateField(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteField(id, mutationId);

        private async Task<IEnumerable<Field>> GetRaw(GetFieldsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetFields(parameters);
            List<Field> fields = [];

            foreach (DataRow row in table.Rows)
            {
                Field field = new()
                {
                    Id = new Guid(row["fld_id"].ToString()!),
                    Shop = new Shop()
                    {
                        Id = new Guid(row["fld_shop"].ToString()!),
                        Name = Utilities.DbNullableString(row["fld_shop_name"]),
                        MerchantId = new Guid(row["fld_shop_merchant"].ToString()!)
                    },
                    Name = Utilities.DbNullableString(row["fld_name"]),

                    Entity = (FieldEntity)Convert.ToInt32(row["fld_entity"]),
                    Type = (FieldType)Convert.ToInt32(row["fld_type"]),
                    UserDefinedMandatory = Convert.ToBoolean(row["fld_user_defined_mandatory"]),
                    DataType = (FieldDataType)Convert.ToInt32(row["fld_data_type"]),
                    SortOrder = Utilities.DbNullableInt(row["fld_sortorder"]),
                    Visible = Convert.ToBoolean(row["fld_visible"])
                };

                if (row["fld_enumerations"] != DBNull.Value)
                {
                    string enumeration = Utilities.DbNullableString(row["fld_enumerations"]);
                    field.Enumerations = enumeration.Split(Environment.NewLine);
                }

                fields.Add(field);
            }

            return fields;
        }

        private async Task<IEnumerable<PublicField>> GetRawPublic(GetFieldsParameters parameters)
        {
            // Only get visible fields
            parameters.Visible = true;

            DataTable table = await _dataAccessLayer.GetFields(parameters);
            List<PublicField> fields = [];

            foreach (DataRow row in table.Rows)
            {
                PublicField field = new()
                {
                    Id = new Guid(row["fld_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["fld_name"]),
                    DataType = (FieldDataType)Convert.ToInt32(row["fld_data_type"]),
                    UserDefinedMandatory = Convert.ToBoolean(row["fld_user_defined_mandatory"]),
                };

                if (row["fld_enumerations"] != DBNull.Value)
                {
                    string enumeration = Utilities.DbNullableString(row["fld_enumerations"]);
                    field.Enumerations = enumeration.Split(Environment.NewLine);
                }

                fields.Add(field);
            }

            return fields;
        }
    }
}