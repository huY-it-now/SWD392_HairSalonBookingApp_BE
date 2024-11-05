using Application.Interfaces;
using Application.Repositories;
using Domain.Contracts.DTO.Combo;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ComboServiceComboDetailService : IComboServiceComboDetail
    {
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;

        public ComboServiceComboDetailService(IComboServiceComboDetailRepository comboServiceComboDetailRepository)
        {
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
        }

        public async Task<List<ComboDetail>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            //lấy danh sách ComboDetails theo comboServiceId
            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(comboServiceId);

            return comboDetails.ToList();
        }

        public async Task<List<ComboService>> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            //lấy danh sách ComboServices theo comboDetailId
            var comboServices = await _comboServiceComboDetailRepository.GetComboServicesByComboDetailId(comboDetailId);

            return comboServices.ToList();
        }
    }
}
