using Application.Repositories;
using Domain.Contracts.DTO.Combo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ComboServiceComboDetailService
    {
        private readonly IComboServiceComboDetailRepository _comboServiceComboDetailRepository;

        public ComboServiceComboDetailService(IComboServiceComboDetailRepository comboServiceComboDetailRepository)
        {
            _comboServiceComboDetailRepository = comboServiceComboDetailRepository;
        }

        public async Task<List<ComboDetailDTO>> GetComboDetailsByComboServiceId(Guid comboServiceId)
        {
            var comboDetails = await _comboServiceComboDetailRepository.GetComboDetailsByComboServiceId(comboServiceId);
            return comboDetails.Select(cd => new ComboDetailDTO
            {
                Id = cd.Id,
                Content = cd.Content 
            }).ToList();
        }

        public async Task<List<ComboServiceDTO>> GetComboServicesByComboDetailId(Guid comboDetailId)
        {
            var comboServices = await _comboServiceComboDetailRepository.GetComboServicesByComboDetailId(comboDetailId);
            return comboServices.Select(cs => new ComboServiceDTO
            {
                Id = cs.Id,
                ComboServiceName = cs.ComboServiceName,
                Price = cs.Price,
                ImageUrl = cs.ImageUrl, 
                SalonId = cs.SalonId
            }).ToList();
        }
    }
}
