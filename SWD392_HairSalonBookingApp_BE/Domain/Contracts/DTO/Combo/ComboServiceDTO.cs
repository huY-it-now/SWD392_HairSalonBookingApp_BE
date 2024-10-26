using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Domain.Contracts.DTO.Combo
{
    public class ComboServiceDTO
    {
        public Guid Id { get; set; }
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public List<ComboServiceComboDetail> ComboDetails { get; set; }
    }
}
