using Application.Interfaces;
using AutoMapper;
using Domain.Contracts.Abstracts.Service;
using Domain.Contracts.Abstracts.Shared;
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
    }
}
