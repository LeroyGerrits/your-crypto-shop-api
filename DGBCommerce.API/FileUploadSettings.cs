namespace DGBCommerce.API
{
    public class FileUploadSettings
    {
        public string? AllowedExtensions { get; set; }
        public string? BaseFolder { get; set; }
        public int? MaximumFileSize { get; set; }        
    }
}
