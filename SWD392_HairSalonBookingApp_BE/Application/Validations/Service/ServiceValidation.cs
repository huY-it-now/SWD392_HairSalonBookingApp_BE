using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Combo;
using Domain.Contracts.DTO.Service;
using FluentValidation;

namespace Application.Validations.Service
{
    public class ServiceValidation : AbstractValidator<CreateServiceDTO>
    {
        public ServiceValidation()
        {
            RuleFor(x => x.ServiceId)
                .NotEqual(Guid.Empty)
                .WithMessage("ServiceId is required");

            RuleFor(x => x.ServiceName)
                .NotEmpty()
                .WithMessage("ServiceName is required");
        }
    }
}
