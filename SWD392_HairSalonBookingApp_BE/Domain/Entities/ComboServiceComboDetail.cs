using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ComboServiceComboDetail
    {
        public Guid ComboServiceId { get; set; }
        public ComboService ComboService { get; set; }
        public Guid ComboDetailId { get; set; }
        public ComboDetail ComboDetail { get; set; }
    }
}
