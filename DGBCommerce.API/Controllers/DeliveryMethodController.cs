using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;

        public DeliveryMethodController(IDeliveryMethodRepository deliveryMethodRepository)
        {
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> Get()
        {
            IEnumerable<DeliveryMethod> deliveryMethods = await _deliveryMethodRepository.Get();
            return Ok(deliveryMethods.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Get(Guid id)
        {
            DeliveryMethod deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            return Ok(deliveryMethod);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DeliveryMethod value)
        {
            var result = await _deliveryMethodRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] DeliveryMethod value)
        {
            DeliveryMethod deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            var result = await _deliveryMethodRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(deliveryMethod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Delete(Guid id)
        {
            DeliveryMethod deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            var result = await _deliveryMethodRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(deliveryMethod);
        }
    }
}