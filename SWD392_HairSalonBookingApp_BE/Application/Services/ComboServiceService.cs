using Application.Interfaces;
using Application.Repositories;
using Application.Validations.Combo;
using AutoMapper;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;

namespace Application.Services
{
    public class ComboServiceService : IComboServiceService
    {
        readonly IComboServiceRepository _comboServiceRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ComboServiceService(IComboServiceRepository comboServiceRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _comboServiceRepository = comboServiceRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllComboServices()
        {
            var cbs = await _comboServiceRepository.GetAllComboServiceAsync();
            var cbsMapper = _mapper.Map<List<ComboServiceDTO>>(cbs);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all comboservice",
                Data = cbsMapper
            };
        }

        public async Task<Result<object>> GetComboServiceById(Guid id)
        {
            var cbs = await _comboServiceRepository.GetComboServiceById(id);

            if (cbs == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Combo service not found",
                    Data = null
                };
            }

            var comboServiceDTO = _mapper.Map<ComboServiceDTO>(cbs);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service details",
                Data = comboServiceDTO
            };
        }

        public async Task<Result<object>> AddComboService(AddComboServiceRequest createRequest)
        {
            ComboServiceValidation.Validate(_mapper.Map<ComboServiceDTO>(createRequest));
            var comboService = _mapper.Map<ComboService>(createRequest);
            await _comboServiceRepository.AddComboService(comboService);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service added successfully",
                Data = null
            };
        }

        public async Task<Result<object>> UpdateComboService(UpdateComboServiceRequest updateRequest)
        {
            ComboServiceValidation.Validate(_mapper.Map<ComboServiceDTO>(updateRequest));
            var comboService = _mapper.Map<ComboService>(updateRequest);
            await _comboServiceRepository.UpdateComboService(comboService);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service updated successfully",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteComboService(Guid id)
        {
            await _comboServiceRepository.DeleteComboService(id);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo service deleted successfully",
                Data = null
            };
        }

        public Task DeleteComboServiceAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateComboServiceAsync(ComboService comboService)
        {
            throw new NotImplementedException();
        }

        public Task<ComboService> GetComboServiceByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ComboService>> GetAllComboServicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ComboService> CreateComboServiceAsync(ComboService comboService)
        {
            throw new NotImplementedException();
        }
    }
}
