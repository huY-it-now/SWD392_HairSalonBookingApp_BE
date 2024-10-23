using Application.Interfaces;
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
        private readonly IComboDetail _comboDetailService;
        private readonly IComboService _comboServiceService;

        public ComboController(IComboDetail comboDetailService, IComboService comboServiceService)
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
            if (result.Data == null || ((IEnumerable<ComboDetailDTO>)result.Data).Any() == false)
            {
                return NotFound(new { Error = 1, Message = "No combo detail found" });
            }
            return Ok(result);
        }

        [HttpGet("get-comboDetails/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboDetailById(Guid id)
        {
            var result = await _comboDetailService.GetComboDetailById(id);
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }
            return Ok(result);
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
                var result = await _comboDetailService.AddComboDetail(createRequest);
                return Ok(result);
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
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }

            try
            {
                var updateResult = await _comboDetailService.UpdateComboDetail(updateRequest);
                return Ok(updateResult);
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
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo detail not found" });
            }

            var deleteResult = await _comboDetailService.DeleteComboDetail(id);
            return Ok(deleteResult);
        }

        [HttpGet("get-comboServiceByComboDetail/{comboDetailId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            var result = await _comboDetailService.GetComboServicesByComboDetailId(comboDetailId);
            if (result.Data == null || ((IEnumerable<ComboServiceDTO>)result.Data).Any() == false)
            {
                return NotFound(new { Error = 1, Message = "No combo services found for this combo detail" });
            }
            return Ok(result);
        }

        // ComboService API methods

        [HttpGet("getAll-comboServices")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllComboServices()
        {
            var result = await _comboServiceService.GetAllComboServices();
            if (result.Data == null || ((IEnumerable<ComboServiceDTO>)result.Data).Any() == false)
            {
                return NotFound(new { Error = 1, Message = "No combo service found" });
            }

            return Ok(result);
        }

        [HttpGet("get-comboServices/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboServiceById(Guid id)
        {
            var result = await _comboServiceService.GetComboServiceById(id);
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

            return Ok(result);
        }

        [HttpPost("add-comboServices")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> AddComboService([FromForm] AddComboServiceRequest createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = 1, Message = "Invalid data" });
            }

            try
            {
                var result = await _comboServiceService.AddComboService(createRequest);
                return Ok(result);
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
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

            try
            {
                var updateResult = await _comboServiceService.UpdateComboService(updateRequest);
                return Ok(updateResult);
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
            if (result.Data == null)
            {
                return NotFound(new { Error = 1, Message = "Combo service not found" });
            }

            var deleteResult = await _comboServiceService.DeleteComboService(id);
            return Ok(deleteResult);
        }


        [HttpGet("get-comboDetailsByComboService/{comboServiceId}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(404, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            var result = await _comboServiceService.GetComboDetailsByComboServiceId(comboServiceId);
            if (result.Data == null || ((IEnumerable<ComboDetailDTO>)result.Data).Any() == false)
            {
                return NotFound(new { Error = 1, Message = "No combo details found for this service" });
            }
            return Ok(result);
        }
    }
}
