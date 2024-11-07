using System;
using System.Collections.Generic;

namespace Domain.Contracts.DTO.Combo
{
    public class ComboDetailDTO
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public bool IsDeleted { get; set; }
    }
}
