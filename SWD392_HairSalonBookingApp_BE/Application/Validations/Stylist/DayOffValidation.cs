using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Stylist;
using FluentValidation;

namespace Application.Validations.Stylist
{
    public class DayOffValidation : AbstractValidator<RegisterDayOffDTO>
    {
        public DayOffValidation()
        {
            RuleFor(x => x.StylistId)
            .NotEmpty()
            .WithMessage("Stylist ID is required.");

            RuleFor(x => x.DayOffDate)
                .NotEmpty()
                .WithMessage("Day off date is required.");

            RuleFor(x => x)
                .Must(dto => dto.IsFullDay ? dto.DayOffDate.Date >= DateTime.UtcNow.Date : true)
                .WithMessage("If the day off is full, it must be today or a future date.");
        }
    }
}
