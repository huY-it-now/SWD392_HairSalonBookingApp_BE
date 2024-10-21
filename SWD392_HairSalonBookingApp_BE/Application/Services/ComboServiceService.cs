using Application.Interfaces;
using Application.Repositories;
using Application.Validations.Combo;
using AutoMapper;
using Domain.Contracts.Abstracts.Cloudinary;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ComboServiceService : IComboService
    {
        readonly IComboServiceRepository _comboServiceRepository;
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;

        public ComboServiceService(IComboServiceRepository comboServiceRepository,
                                   IComboServiceComboDetailRepository comboServiceComboDetailRepository,
                                   IMapper mapper,
                                   IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
        {
            _comboServiceRepository = comboServiceRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<Result<object>> GetAllComboServices()
        {
            var cbs = await _comboServiceRepository.GetAllComboServiceAsync();
            var cbsMapper = _mapper.Map<List<ComboServiceDTO>>(cbs);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all combo services",
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

            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(id);
            var comboDetailDTOs = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

            var comboServiceDTO = _mapper.Map<ComboServiceDTO>(cbs);
            comboServiceDTO.ComboDetails = comboDetailDTOs;

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

            string fileExtension = Path.GetExtension(createRequest.ImageUrl.FileName);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            CloudinaryResponse cloudinaryResult = await _cloudinaryService.UploadImage(newFileName, createRequest.ImageUrl);

            if (cloudinaryResult == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Error uploading image. Please try again.",
                    Data = null
                };
            }

            var comboService = _mapper.Map<ComboService>(createRequest);
            await _comboServiceRepository.AddComboService(comboService);
            await _unitOfWork.SaveChangeAsync();

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

        public async Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(comboServiceId);
            var comboDetailDTOs = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo details for the combo service",
                Data = comboDetailDTOs
            };
        }
    }
}
