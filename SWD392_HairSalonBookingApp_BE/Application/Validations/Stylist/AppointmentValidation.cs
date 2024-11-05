using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Appointment;
using FluentValidation;

namespace Application.Validations.Stylist
{
    public class AppointmentValidation : AbstractValidator<UpdateBookingStatusDTO>
    {
        public AppointmentValidation()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty().WithMessage("Appointment ID is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Pending" || status == "Completed" || status == "Canceled")
                .WithMessage("Status must be 'Pending', 'Completed', or 'Canceled'.");
        }
    }
}
