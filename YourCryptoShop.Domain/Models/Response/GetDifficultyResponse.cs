namespace YourCryptoShop.Domain.Models.Response
{
    public class GetDifficultyResponse
    {
        public required GetDifficultyResponseDifficulties Difficulties { get; set; }
    }
}