using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.DTO.Appointment;
using FluentValidation;

namespace Application.Validations.Stylist
{
    public class BookingValidation : AbstractValidator<UpdateBookingStatusDTO>
    {
        public BookingValidation()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("Booking ID is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "In Progress" || status == "Completed")
                .WithMessage("Status must be 'In Progress' or 'Completed'.");
        }
    }
}
