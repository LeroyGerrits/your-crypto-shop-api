using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductPhotoController : ControllerBase
    {
        private readonly FileUploadSettings _fileUploadSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IProductPhotoRepository _productPhotoRepository;

        public ProductPhotoController(
            IOptions<FileUploadSettings> fileUploadSettings,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IProductPhotoRepository productPhotoRepository)
        {
            _fileUploadSettings = fileUploadSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _productPhotoRepository = productPhotoRepository;
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductPhoto>>> Get(Guid? productId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var productPhotos = await _productPhotoRepository.Get(new GetProductPhotosParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ProductId = productId
            });
            return Ok(productPhotos.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductPhoto>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var productPhoto = await _productPhotoRepository.GetById(authenticatedMerchantId.Value, id);
            if (productPhoto == null)
                return NotFound();

            return Ok(productPhoto);
        }

        [AuthenticationRequired]
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Post(Guid productId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            if (string.IsNullOrWhiteSpace(_fileUploadSettings.BaseFolder))
                return BadRequest(new { message = "Base folder not configured." });

            var formCollection = await Request.ReadFormAsync();
            if (formCollection.Files.Count == 0)
                return BadRequest(new { message = "No file uploaded." });

            var file = formCollection.Files[0];
            if (file.Length == 0)
                return BadRequest(new { message = "Empty file uploaded." });

            if (_fileUploadSettings.MaximumFileSize.HasValue && file.Length > _fileUploadSettings.MaximumFileSize)
                return BadRequest(new { message = "File was too large. The maximum allowed file size is " + Utilities.ReadableFileSize(_fileUploadSettings.MaximumFileSize.Value) + "." });

            var fileExtension = Path.GetExtension(file.FileName).Replace(".", string.Empty);

            if (!string.IsNullOrWhiteSpace(_fileUploadSettings.AllowedExtensions) && !_fileUploadSettings.AllowedExtensions.ToUpper().Contains(fileExtension.ToUpper()))
                return BadRequest(new { message = "File type " + fileExtension + " is not allowed. Only the following file types are allowed: " + _fileUploadSettings.AllowedExtensions + "." });

            var folder = Path.Combine(_fileUploadSettings.BaseFolder, authenticatedMerchantId.Value.ToString());
            folder = Path.Combine(folder, "ProductPhoto");

            if (!Path.Exists(folder))
                Directory.CreateDirectory(folder);

            var newFileId = Guid.NewGuid();
            var fullPath = Path.Combine(folder, $"{newFileId}.{fileExtension}");

            using FileStream stream = new(fullPath, FileMode.Create);
            file.CopyTo(stream);

            ProductPhoto productPhotoToCreate = new()
            {
                Id = newFileId,
                ProductId = productId,
                File = file.FileName.Replace("." + fileExtension, string.Empty),
                Extension = fileExtension.ToLower(),
                FileSize = (int)file.Length,
                Width = 100,
                Height = 100,
                Visible = true
            };

            var result = await _productPhotoRepository.Create(productPhotoToCreate, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] ProductPhoto value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var productPhoto = await _productPhotoRepository.GetById(authenticatedMerchantId.Value, id);
            if (productPhoto == null)
                return NotFound();

            var result = await _productPhotoRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductPhoto>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var productPhoto = await _productPhotoRepository.GetById(authenticatedMerchantId.Value, id);
            if (productPhoto == null)
                return NotFound();

            var result = await _productPhotoRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}