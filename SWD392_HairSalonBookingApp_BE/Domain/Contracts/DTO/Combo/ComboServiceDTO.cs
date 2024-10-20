using System;
using System.Collections.Generic;

namespace Domain.Contracts.DTO.Combo
{
    public class ComboServiceDTO
    {
        public Guid Id { get; set; }
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public Guid SalonId { get; set; }
        public string ImageUrl { get; set; }
        public List<ComboDetailDTO> ComboDetails { get; set; }
    }
}
