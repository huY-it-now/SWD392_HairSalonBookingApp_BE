using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly ComboServiceService _comboServiceService;
        private readonly ComboDetailService _comboDetailService;

        public ComboController(ComboServiceService comboServiceService, ComboDetailService comboDetailService)
        {
            _comboServiceService = comboServiceService;
            _comboDetailService = comboDetailService;
        }

        // CRUD cho ComboService

        [HttpPost("service")]
        public async Task<IActionResult> CreateComboService([FromBody] ComboService comboService)
        {
            var createdComboService = await _comboServiceService.CreateComboServiceAsync(comboService);
            return CreatedAtAction(nameof(GetComboServiceById), new { id = createdComboService.Id }, createdComboService);
        }

        [HttpGet("service")]
        public async Task<IActionResult> GetAllComboServices()
        {
            var comboServices = await _comboServiceService.GetAllComboServicesAsync();
            return Ok(comboServices);
        }

        [HttpGet("service/{id:guid}")]
        public async Task<IActionResult> GetComboServiceById(Guid id)
        {
            var comboService = await _comboServiceService.GetComboServiceByIdAsync(id);
            if (comboService == null)
            {
                return NotFound();
            }
            return Ok(comboService);
        }

        [HttpPut("service/{id:guid}")]
        public async Task<IActionResult> UpdateComboService(Guid id, [FromBody] ComboService comboService)
        {
            if (id != comboService.Id)
            {
                return BadRequest("Id mismatch");
            }

            await _comboServiceService.UpdateComboServiceAsync(comboService);
            return NoContent();
        }

        [HttpDelete("service/{id:guid}")]
        public async Task<IActionResult> DeleteComboService(Guid id)
        {
            await _comboServiceService.DeleteComboServiceAsync(id);
            return NoContent();
        }

        // CRUD cho ComboDetail

        [HttpPost("detail")]
        public async Task<IActionResult> CreateComboDetail([FromBody] ComboDetail comboDetail)
        {
            var createdComboDetail = await _comboDetailService.CreateComboDetailAsync(comboDetail);
            return CreatedAtAction(nameof(GetComboDetailById), new { id = createdComboDetail.Id }, createdComboDetail);
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetAllComboDetails()
        {
            var comboDetails = await _comboDetailService.GetAllComboDetailsAsync();
            return Ok(comboDetails);
        }

        [HttpGet("detail/{id:guid}")]
        public async Task<IActionResult> GetComboDetailById(Guid id)
        {
            var comboDetail = await _comboDetailService.GetComboDetailByIdAsync(id);
            if (comboDetail == null)
            {
                return NotFound();
            }
            return Ok(comboDetail);
        }

        [HttpPut("detail/{id:guid}")]
        public async Task<IActionResult> UpdateComboDetail(Guid id, [FromBody] ComboDetail comboDetail)
        {
            if (id != comboDetail.Id)
            {
                return BadRequest("Id mismatch");
            }

            await _comboDetailService.UpdateComboDetailAsync(comboDetail);
            return NoContent();
        }

        [HttpDelete("detail/{id:guid}")]
        public async Task<IActionResult> DeleteComboDetail(Guid id)
        {
            await _comboDetailService.DeleteComboDetailAsync(id);
            return NoContent();
        }
    }
}
