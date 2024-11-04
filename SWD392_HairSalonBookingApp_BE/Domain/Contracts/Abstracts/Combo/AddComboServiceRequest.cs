using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Combo
{
    public class AddComboServiceRequest
    {
        public string ComboServiceName { get; set; }
        public decimal Price { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public List<Guid> ComboDetailIds { get; set; }
    }
}
