using Application.Services;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComboController : ControllerBase
    {
        private readonly ComboDetailService _comboDetailService;
        private readonly ComboServiceService _comboServiceService;

        public ComboController(ComboDetailService comboDetailService, ComboServiceService comboServiceService)
        {
            _comboDetailService = comboDetailService;
            _comboServiceService = comboServiceService;
        }

        // ComboDetail API methods

        [HttpGet("getAll-comboDetails")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
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

        [HttpGet("get-comboDetails/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboDetailById(Guid id)
        {
            var result = await _comboDetailService.GetComboDetailById(id);
            var comboDetail = result.Data as ComboDetailDTO;

            if (comboDetail == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }

            return Ok(new { Error = 0, Message = "Combo detail details", Data = comboDetail });
        }

        [HttpPost("add-comboDetails")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddComboDetail([FromBody] AddComboDetailRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = 1, Message = "Invalid data" });
            }

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

        [HttpPut("update-comboDetails/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateComboDetail(Guid id, [FromBody] UpdateComboDetailRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = 1, Message = "Invalid data" });
            }

            var result = await _comboDetailService.GetComboDetailById(id);
            if (result == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }

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

        [HttpDelete("delete-comboDetails/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteComboDetail(Guid id)
        {
            var result = await _comboDetailService.GetComboDetailById(id);
            if (result == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }

            await _comboDetailService.DeleteComboDetail(id);
            return Ok(new { Error = 0, Message = "Combo detail deleted successfully" });
        }

        // ComboService API methods

        [HttpGet("getAll-comboServices")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllComboServices()
        {
            var result = await _comboServiceService.GetAllComboServices();
            var comboServices = result.Data as IEnumerable<ComboServiceDTO>;

            if (comboServices == null || !comboServices.Any())
            {
                return NotFound(new { Error = 1, Message = "No combo service found" });
            }

            return Ok(new { Error = 0, Message = "All combo services", Data = comboServices });
        }

        [HttpGet("get-comboServices/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboServiceById(Guid id)
        {
            var result = await _comboServiceService.GetComboServiceById(id);
            var comboService = result.Data as ComboServiceDTO;

            if (comboService == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

            return Ok(new { Error = 0, Message = "Combo service details", Data = comboService });
        }

        [HttpPost("add-comboServices")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddComboService([FromBody] AddComboServiceRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = 1, Message = "Invalid data" });
            }

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

        [HttpPut("update-comboServices/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateComboService(Guid id, [FromBody] UpdateComboServiceRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = 1, Message = "Invalid data" });
            }

            var result = await _comboServiceService.GetComboServiceById(id);
            if (result == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

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

        [HttpDelete("delete-comboServices/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteComboService(Guid id)
        {
            var result = await _comboServiceService.GetComboServiceById(id);
            if (result == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

            await _comboServiceService.DeleteComboService(id);
            return Ok(new { Error = 0, Message = "Combo service deleted successfully" });
        }
    }
}
