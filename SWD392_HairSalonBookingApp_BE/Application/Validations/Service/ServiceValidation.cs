using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.DTO.Service;

namespace Application.Validations.Service
{
    public static class ServiceValidation
    {
        public static void Validate(ServiceDTO serviceDTO)
        {
            if (string.IsNullOrEmpty(serviceDTO.ServiceName))
            {
                throw new ArgumentException("Service name is required");
            }
            if (serviceDTO.Id == Guid.Empty)
            {
                throw new ArgumentException("ServiceId is required");
            }
        }
    }
}
