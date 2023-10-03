namespace DGBCommerce.Domain.Models.Response
{
    public class GetDifficultyResponse
    {
        public required GetDifficultyResponseDifficulties Difficulties { get; set; }

        public class GetDifficultyResponseDifficulties
        {
            public double Sha256d { get; set; }
            public double Scrypt { get; set; }
            public double Skein { get; set; }
            public double Qubit { get; set; }
            public double Odo { get; set; }
        }
    }
}
