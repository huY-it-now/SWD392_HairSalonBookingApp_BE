using Domain.Contracts.Abstracts.Account;
using FluentValidation;

namespace Application.Validations.Account
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required!").MinimumLength(1).WithMessage("At least 6 characters!");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required!").MinimumLength(10).MaximumLength(11).WithMessage("Phone number must have 10 or 11 numbers");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!").EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required!").Equal(x => x.Password).WithMessage("Is not equal with password");
        }
    }
}
