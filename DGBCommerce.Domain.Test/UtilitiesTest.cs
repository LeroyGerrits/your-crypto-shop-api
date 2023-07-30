namespace DGBCommerce.Domain.Test
{
    public class UtilitiesTest
    {
        [Fact]
        public void Hashing_test_should_return_hashed_output()
        {
            string value = "Test";
            string valueHashed = Utilities.HashStringSha256(value);

            Assert.Equal("532eaabd9574880dbf76b9b8cc00832c20a6ec113d682299550d7a6e0f345e25", valueHashed);
        }

        [Fact]
        public void Generate_salt_should_return_a_32_bit_hash()
        {
            object valueHash = Utilities.GenerateSalt();
            Assert.NotNull(valueHash);
        }

        [Fact]
        public void Empty_datetime_from_database_should_evaluate_no_null()
        {
            object value = DBNull.Value;
            object valueNullable = Utilities.DBNullableDateTime(value)!;

            Assert.Null(valueNullable);
        }
    }
}