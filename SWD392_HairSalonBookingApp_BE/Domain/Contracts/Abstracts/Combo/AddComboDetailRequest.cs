using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Combo
{
    public class AddComboDetailRequest
    {
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public Guid ComboServiceId { get; set; }
    }
}
