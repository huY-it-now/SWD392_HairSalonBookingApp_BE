using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Combo
{
    public class UpdateComboServiceRequest
    {
        public Guid Id { get; set; }
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
    }
}
