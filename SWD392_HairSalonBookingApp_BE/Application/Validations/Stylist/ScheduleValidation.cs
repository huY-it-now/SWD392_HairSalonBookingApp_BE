using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Stylish;
using FluentValidation;

namespace Application.Validations.Stylish
{
    public class ScheduleValidation : AbstractValidator<ScheduleDTO>
    {
        public ScheduleValidation()
        {
            RuleFor(x => x.StylistId)
                .NotEmpty().WithMessage("Stylist ID is required.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .GreaterThan(DateTime.MinValue).WithMessage("Invalid date.");

            RuleFor(x => x.WorkShift)
                .NotEmpty().WithMessage("Work shift is required.")
                .Must(shift => shift == "Morning" || shift == "Afternoon" || shift == "Evening")
                .WithMessage("Work shift must be either 'Morning', 'Afternoon', or 'Evening'.");
        }
    }
}
