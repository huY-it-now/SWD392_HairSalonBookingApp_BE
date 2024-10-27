using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
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
using Domain.Contracts.DTO.Combo;

namespace Application.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceRepository _serviceRepository;

        public ServiceService(IMapper mapper, IUnitOfWork unitOfWork, IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllServices()
        {
            var services = await _unitOfWork.ServiceRepository.GetAllServicesAsync();

            var servicesDTOList = services.Select(service => new ServiceDTO
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                ComboServiceDTOs = service.ServiceComboServices?
                    .Where(scs => scs.ComboService != null)
                    .Select(scs => new ComboServiceDTO
                    {
                        Id = scs.ComboService.Id,
                        ComboServiceName = scs.ComboService.ComboServiceName,
                        Price = scs.ComboService.Price,
                        Image = scs.ComboService.ImageUrl,
                        ComboDetails = scs.ComboService.ComboServiceComboDetails?
                            .Where(cd => cd.ComboDetail != null)
                            .Select(cd => new ComboDetailDTO
                            {
                                Id = cd.ComboDetail.Id,
                                Content = cd.ComboDetail.Content
                            }).ToList() ?? new List<ComboDetailDTO>()
                    }).ToList() ?? new List<ComboServiceDTO>()
            }).ToList();

            return new Result<object>
            {
                Error = 0,
                Message = "Print all services with their associated combo services and combo details",
                Data = servicesDTOList
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

            var serviceDTO = new ServiceDTO
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                ComboServiceDTOs = service.ServiceComboServices?
                    .Where(scs => scs.ComboService != null)
                    .Select(scs => new ComboServiceDTO
                    {
                        Id = scs.ComboService.Id,
                        ComboServiceName = scs.ComboService.ComboServiceName,
                        Price = scs.ComboService.Price,
                        Image = scs.ComboService.ImageUrl,
                        ComboDetails = scs.ComboService.ComboServiceComboDetails?
                            .Where(cd => cd.ComboDetail != null)
                            .Select(cd => new ComboDetailDTO
                            {
                                Id = cd.ComboDetail.Id,
                                Content = cd.ComboDetail.Content
                            }).ToList() ?? new List<ComboDetailDTO>()
                    }).ToList() ?? new List<ComboServiceDTO>()
            };

            return new Result<object>
            {
                Error = 0,
                Message = "Service details retrieved successfully!",
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

            var service = new Service
            {
                Id = Guid.NewGuid(),
                ServiceName = createRequest.ServiceName,
                CategoryId = createRequest.CategoryId,
                IsDeleted = false
            };

            try
            {
                await _unitOfWork.ServiceRepository.CreateService(service);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (ArgumentException ex)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = ex.Message, 
                    Data = null
                };
            }
            catch (Exception)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "An error occurred while creating the service.",
                    Data = null
                };
            }

            var serviceDTO = _mapper.Map<ServiceDTO>(service);

            return new Result<object>
            {
                Error = 0,
                Message = "Service created successfully!",
                Data = serviceDTO
            };

        }

        public async Task<Result<object>> UpdateService(Guid id, UpdateServiceDTO updateRequest)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceById(id);

            if (service == null || service.IsDeleted)
            {
                return new Result<object> 
                {
                    Error = 1,
                    Message = "Service not found or has been deleted!",
                    Data = null
                };
            }

            service.ServiceName = updateRequest.ServiceName;
            service.CategoryId = updateRequest.CategoryId;

            try
            {
                _unitOfWork.ServiceRepository.Update(service);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "An error occurred while updating the category.",
                    Data = null
                };
            }

            var serviceDTO = _mapper.Map<ServiceDTO>(service);

            return new Result<object>
            {
                Error = 0,
                Message = "Service updated successfully!",
                Data = serviceDTO
            };
        }

        public async Task<Result<object>> DeleteService(Guid id)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceById(id);

            if (service == null || service.IsDeleted)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Service not found or has already been deleted!",
                    Data = null
                };
            }

            service.IsDeleted = true;
            _unitOfWork.ServiceRepository.Update(service);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Service marked as deleted successfully!",
                Data = null
            };

        }

        public async Task<Result<object>> AddComboIntoService(Guid serviceId, Guid comboServiceId)
        {
            var service = await _unitOfWork.ServiceRepository.GetServiceById(serviceId);
            var comboService = await _unitOfWork.ComboServiceRepository.GetComboServiceById(comboServiceId);

            if (service == null || comboService == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found",
                };
            }

            var serviceComboService = new ServiceComboService
            {
                Service = service,
                ServiceId = serviceId,
                ComboService = comboService,
                ComboServiceId = comboServiceId
            };

            await _unitOfWork.ServiceComboServiceRepository.AddAsync(serviceComboService);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Add combo service into service successfull",
                Data = null
            };
        }
    }
}
