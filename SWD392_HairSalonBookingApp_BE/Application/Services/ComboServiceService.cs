using Application.Repositories;
using Application.Validations.Combo;
using AutoMapper;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;

namespace Application.Services
{
    public class ComboServiceService
    {
        readonly IComboServiceRepository _comboServiceRepository;
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository; // Thêm repository cho bảng trung gian
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ComboServiceService(IComboServiceRepository comboServiceRepository,
                                   IComboServiceComboDetailRepository comboServiceComboDetailRepository, // Thêm repo cho bảng trung gian
                                   IMapper mapper,
                                   IUnitOfWork unitOfWork)
        {
            _comboServiceRepository = comboServiceRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
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

            // Lấy danh sách ComboDetail thông qua ComboServiceComboDetail
            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(id);
            var comboDetailDTOs = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

            var comboServiceDTO = _mapper.Map<ComboServiceDTO>(cbs);
            comboServiceDTO.ComboDetails = comboDetailDTOs; // Gán danh sách combo details vào DTO

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
    }
}
