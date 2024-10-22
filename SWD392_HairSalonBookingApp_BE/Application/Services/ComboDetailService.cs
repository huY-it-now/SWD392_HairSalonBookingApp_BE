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

        public ComboDetailService(IComboDetailRepository comboDetailRepository,
                                  IComboServiceComboDetailRepository comboServiceComboDetailRepository,
                                  IMapper mapper)
        {
            _comboDetailRepository = comboDetailRepository;
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
            _mapper = mapper;
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
            comboDetailDTO.ComboServices = comboServiceDTOs;

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
            ComboDetailValidation.Validate(_mapper.Map<ComboDetailDTO>(createRequest));
            var comboDetail = _mapper.Map<ComboDetail>(createRequest);
            await _comboDetailRepository.AddComboDetail(comboDetail);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo detail added successfully",
                Data = null
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
            await _comboDetailRepository.DeleteComboDetail(id);

            return new Result<object>
            {
                Error = 0,
                Message = "Combo detail deleted successfully",
                Data = null
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
