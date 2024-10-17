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
        public static void Validate(CreateServiceDTO createRequest)
        {
            if (createRequest.ServiceId == Guid.Empty)
            {
                throw new ArgumentException("ServiceId is required");
            }
            if (string.IsNullOrWhiteSpace(createRequest.ServiceName))
            {
                throw new ArgumentException("ServiceName is required");
            }
        }
    }
}
