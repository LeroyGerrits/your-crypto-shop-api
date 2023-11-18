using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductPhotoController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IProductPhotoRepository _productPhotoRepository;

        public ProductPhotoController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IProductPhotoRepository productPhotoRepository)
        {
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

            try
            {
                var formCollection = await Request.ReadFormAsync();
                for (int i = 0; i < formCollection.Files.Count; i++)
                {
                    var file = formCollection.Files[i];
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                    folder = Path.Combine(folder, "ProductPhoto");
                    folder = Path.Combine(folder, authenticatedMerchantId.Value.ToString());

                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');

                        if (!Path.Exists(folder))
                            Directory.CreateDirectory(folder);

                        var fullPath = Path.Combine(folder, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        
                    }
                }

                return Ok(new { message = "boem" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

            //var result = await _productPhotoRepository.Create(productId, authenticatedMerchantId.Value);
            //return Ok(result);
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