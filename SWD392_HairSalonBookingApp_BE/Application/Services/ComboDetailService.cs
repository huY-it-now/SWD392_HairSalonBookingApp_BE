using Application.Interfaces;
using Application.Repositories;
using Application.Validations.Combo;
using AutoMapper;
using Domain.Contracts.Abstracts.Combo;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ComboDetailService : IComboDetail
    {
        private readonly IComboDetailRepository _comboDetailRepository;
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ComboDetailService(IComboDetailRepository comboDetailRepository,
                                  IComboServiceComboDetailRepository comboServiceComboDetailRepository,
                                  IMapper mapper, IUnitOfWork unitOfWork)
        {
            _comboDetailRepository = comboDetailRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<object>> GetAllComboDetails()
        {
            var comboDetails = await _comboDetailRepository.GetAllComboDetailsAsync();
            var comboDetailsMapper = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

            return new Result<object>
            {
                Error = 0,
                Message = "Print all combo details",
                Data = comboDetailsMapper
            };
        }

        public async Task<Result<object>> GetComboDetailById(Guid id)
        {
            var comboDetail = await _comboDetailRepository.GetComboDetailById(id);

            if (comboDetail == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Combo detail not found",
                    Data = null
                };
            }

            var comboServices = await _comboServiceComboDetailRepository.GetComboServicesByComboDetailId(id);
            var comboServiceDTOs = _mapper.Map<List<ComboServiceDTO>>(comboServices);

            var comboDetailDTO = _mapper.Map<ComboDetailDTO>(comboDetail);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo details",
                Data = comboDetailDTO
            };
        }

        public async Task<Result<object>> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            var comboServices = await _comboServiceComboDetailRepository.GetComboServicesByComboDetailId(comboDetailId);
            var comboServiceDTOs = _mapper.Map<List<ComboServiceDTO>>(comboServices);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo services for the combo detail",
                Data = comboServiceDTOs
            };
        }

        public async Task<Result<object>> AddComboDetail(AddComboDetailRequest createRequest)
        {
            var exist = await _unitOfWork.ComboDetailRepository.CheckComboDetailExistByName(createRequest.Content);

            if (exist != null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Combo detail is exist"
                };
            }

            var newComboDetail = _mapper.Map<ComboDetail>(createRequest);

            newComboDetail.Content = createRequest.Content;
            newComboDetail.IsDeleted = false;

            await _unitOfWork.ComboDetailRepository.AddAsync(newComboDetail);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Combo detail added successfully"
            };
        }

        public async Task<Result<object>> UpdateComboDetail(UpdateComboDetailRequest updateRequest)
        {
            ComboDetailValidation.Validate(_mapper.Map<ComboDetailDTO>(updateRequest));
            var comboDetail = _mapper.Map<ComboDetail>(updateRequest);
            await _comboDetailRepository.UpdateComboDetail(comboDetail);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo detail updated successfully",
                Data = null
            };
        }

        public async Task<Result<object>> DeleteComboDetail(Guid id)
        {
            var cbd = await _unitOfWork.ComboDetailRepository.GetComboDetailById(id);

            if (cbd == null)
            {
                return new Result<object>
                {
                    Error = 1,
                    Message = "Not found"
                };
            }

            cbd.IsDeleted = true;

            _unitOfWork.ComboDetailRepository.Update(cbd);
            await _unitOfWork.SaveChangeAsync();

            return new Result<object>
            {
                Error = 0,
                Message = "Delete successfully"
            };
        }

        //public async Task<Result<object>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        //{
        //    var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(comboServiceId);
        //    var comboDetailDTOs = _mapper.Map<List<ComboDetailDTO>>(comboDetails);

        //    return new Result<object>
        //    {
        //        Error = 0,
        //        Message = "Combo details found",
        //        Data = comboDetailDTOs
        //    };
        //}
    }
}
