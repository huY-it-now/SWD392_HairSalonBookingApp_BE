using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Validations.Service;
using AutoMapper;
using Domain.Contracts.Abstracts.Service;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Category;
using Domain.Contracts.DTO.Service;
using Domain.Entities;

namespace Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllServices()
        {
            var services = await _unitOfWork.ServiceRepository.GetAllServicesAsync();
            var servicesMapper = _mapper.Map<List<ServiceDTO>>(services);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all service",
                Data = servicesMapper
            };
        }
        public async Task<Result<object>> GetServiceById(Guid id)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceById(id);

            if (service == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Service not found!",
                    Data = null
                };
            }

            var serviceDTO = _mapper.Map<ServiceDTO>(service);

            return new Result<object>
            {
                Error = 0,
                Message = "Service details etrieved successfully!",
                Data = serviceDTO
            };
        }

        public async Task<Result<object>> CreateService(CreateServiceDTO createRequest)
        {
            var categoryExists = await _unitOfWork.CategoryRepository.GetCategoryById(createRequest.CategoryId);
            if (categoryExists == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Category not found!",
                    Data = null
                };
            }

            var newService = new Service
            {
                Id = Guid.NewGuid(),
                ServiceName = createRequest.ServiceName,
                CategoryId = createRequest.CategoryId,
                IsDeleted = false
            };

            try
            {
                await _unitOfWork.ServiceRepository.AddAsync(newService);
                await _unitOfWork.SaveChangeAsync();

                var serviceDTO = _mapper.Map<ServiceDTO>(newService);

                return new Result<object>
                {
                    Error = 0,
                    Message = "Service created successfully!",
                    Data = serviceDTO
                };
            }
            catch (Exception ex) 
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = $"Failed to create service: {ex.Message}. Inner Exception: {ex.InnerException?.Message}",
                    Data = null
                };
            }
        }
    }
}
