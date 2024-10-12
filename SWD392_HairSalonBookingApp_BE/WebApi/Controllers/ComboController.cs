using Application.Services;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComboController : ControllerBase
    {
        private readonly ComboServiceService _comboServiceService;
        private readonly ComboDetailService _comboDetailService;

        public ComboController(ComboServiceService comboServiceService, ComboDetailService comboDetailService)
        {
            _comboServiceService = comboServiceService;
            _comboDetailService = comboDetailService;
        }

        [HttpGet("comboservices")]
        public async Task<IActionResult> GetAllComboServices()
        {
            var result = await _comboServiceService.GetAllComboServices();
            var comboServices = result.Data as IEnumerable<ComboServiceDTO>;
            if (comboServices == null || !comboServices.Any())
            {
                return NotFound(new { Error = 1, Message = "No combo services found" });
            }
            return Ok(new { Error = 0, Message = "All combo services", Data = comboServices });
        }

        [HttpGet("comboservices/{id}")]
        public async Task<IActionResult> GetComboServiceById(Guid id)
        {
            var comboService = await _comboServiceService.GetComboServiceById(id);
            if (comboService == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }
            return Ok(new { Error = 0, Message = "Combo service details", Data = comboService });
        }

        [HttpPost("comboservices")]
        public async Task<IActionResult> AddComboService([FromBody] AddComboServiceRequest createRequest)
        {
            try
            {
                await _comboServiceService.AddComboService(createRequest);
                return Ok(new { Error = 0, Message = "Combo service added successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpPut("comboservices")]
        public async Task<IActionResult> UpdateComboService([FromBody] UpdateComboServiceRequest updateRequest)
        {
            try
            {
                await _comboServiceService.UpdateComboService(updateRequest);
                return Ok(new { Error = 0, Message = "Combo service updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpDelete("comboservices/{id}")]
        public async Task<IActionResult> DeleteComboService(Guid id)
        {
            await _comboServiceService.DeleteComboService(id);
            return Ok(new { Error = 0, Message = "Combo service deleted successfully" });
        }

        [HttpGet("combodetails")]
        public async Task<IActionResult> GetAllComboDetails()
        {
            var result = await _comboDetailService.GetAllComboDetails();
            var comboDetails = result.Data as IEnumerable<ComboDetailDTO>;
            if (comboDetails == null || !comboDetails.Any())
            {
                return NotFound(new { Error = 1, Message = "No combo detail found" });
            }
            return Ok(new { Error = 0, Message = "All combo details", Data = comboDetails });
        }

        [HttpGet("combodetails/{id}")]
        public async Task<IActionResult> GetComboDetailById(Guid id)
        {
            var comboDetail = await _comboDetailService.GetComboDetailById(id);
            if (comboDetail == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }
            return Ok(new { Error = 0, Message = "Combo detail details", Data = comboDetail });
        }

        [HttpPost("combodetails")]
        public async Task<IActionResult> AddComboDetail([FromBody] AddComboDetailRequest createRequest)
        {
            try
            {
                await _comboDetailService.AddComboDetail(createRequest);
                return Ok(new { Error = 0, Message = "Combo detail added successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpPut("combodetails")]
        public async Task<IActionResult> UpdateComboDetail([FromBody] UpdateComboDetailRequest updateRequest)
        {
            try
            {
                await _comboDetailService.UpdateComboDetail(updateRequest);
                return Ok(new { Error = 0, Message = "Combo detail updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = 1, Message = ex.Message });
            }
        }

        [HttpDelete("combodetails/{id}")]
        public async Task<IActionResult> DeleteComboDetail(Guid id)
        {
            await _comboDetailService.DeleteComboDetail(id);
            return Ok(new { Error = 0, Message = "Combo detail deleted successfully" });
        }
    }
}
