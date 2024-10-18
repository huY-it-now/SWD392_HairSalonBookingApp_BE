using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Contracts.Abstracts.Category;
using Domain.Contracts.Abstracts.Service;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace WebApi.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IServiceService _serviceService;
        private readonly IMapper _mapper;

        public ServiceController(IServiceService serviceService, IMapper mapper)
        {
            _serviceService = serviceService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServices();

            return Ok(services);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetServiceById(id);

            return Ok(service);
        }

        [HttpPost("create-service")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest createRequest)
        {
            var serviceDTO = _mapper.Map<CreateServiceDTO>(createRequest);
            var result = await _serviceService.CreateService(serviceDTO);

            return Ok(result);
        }

        [HttpPut("update-service/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] UpdateServiceRequest updateRequest)
        {
            var updateDTO = _mapper.Map<UpdateServiceDTO>(updateRequest);
            var result = await _serviceService.UpdateService(id, updateDTO);

            return Ok(result);
        }

        [HttpDelete("delete-service/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var result = await _serviceService.DeleteService(id);

            return Ok(result);
        }
    }
}
