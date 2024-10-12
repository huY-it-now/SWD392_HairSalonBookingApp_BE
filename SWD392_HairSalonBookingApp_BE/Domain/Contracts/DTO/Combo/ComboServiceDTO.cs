using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Combo
{
    public class ComboServiceDTO
    {
        public Guid Id { get; set; }
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public Guid SalonId { get; set; }
    }
}
