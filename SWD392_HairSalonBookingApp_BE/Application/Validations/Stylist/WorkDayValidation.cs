using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Stylist;
using FluentValidation;

namespace Application.Validations.Stylist
{
    public class WorkDayValidation : AbstractValidator<RegisterWorkScheduleDTO>
    {
        public WorkDayValidation() 
        {
            RuleFor(x => x.StylistId).NotEmpty().WithMessage("Stylist ID is required.");
            RuleFor(x => x.ScheduleDate).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.WorkShifts)
            .NotEmpty()
            .WithMessage("Work shift is required.")
            .Must(WorkShiftsAreValid)
            .WithMessage("Work shifts must be 'Morning', 'Afternoon', or 'Evening'.");
        }

        private bool WorkShiftsAreValid(List<WorkShiftDTO> workShifts)
        {
            foreach (var shift in workShifts)
            {
                if (shift == null ||
                    (shift.ShiftName != "Morning" && shift.ShiftName != "Afternoon" && shift.ShiftName != "Evening"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
