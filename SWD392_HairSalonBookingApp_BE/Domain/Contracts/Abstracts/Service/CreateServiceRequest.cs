using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Service
{
    public class CreateServiceRequest
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
