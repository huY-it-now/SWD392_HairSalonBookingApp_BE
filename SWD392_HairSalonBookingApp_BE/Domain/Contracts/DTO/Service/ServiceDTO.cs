using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.DTO.Service
{
    public class ServiceDTO
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
