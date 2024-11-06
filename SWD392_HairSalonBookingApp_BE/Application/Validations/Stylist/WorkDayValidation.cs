using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Stylist;
using Domain.Entities;
using FluentValidation;

namespace Application.Validations.Stylist
{
    public class WorkDayValidation : AbstractValidator<RegisterWorkScheduleDTO>
    {
        public WorkDayValidation()
        {
            RuleFor(x => x.StylistId)
                .NotEmpty()
                .WithMessage("Stylist ID is required.");

            RuleFor(x => x.ScheduleDate)
                .NotEmpty()
                .WithMessage("Date is required.");

            RuleFor(x => x.WorkShifts)
                .NotEmpty()
                .WithMessage("Work shifts are required.")
                .Must(shifts => shifts.Count <= 3)
                .WithMessage("You can register up to 3 shifts per day.")
                .Must(shifts => shifts.All(shift =>
                    shift == "Morning" ||
                    shift == "Afternoon" ||
                    shift == "Evening"))
                .WithMessage("Work shift must be 'Morning', 'Afternoon', or 'Evening'.")
                .Must(shifts => shifts
                .Distinct()
                .Count() == shifts.Count)
                .WithMessage("Duplicate work shifts are not allowed.");
        }
    }
}
