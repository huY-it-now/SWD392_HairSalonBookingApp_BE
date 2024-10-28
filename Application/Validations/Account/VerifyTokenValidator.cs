using Domain.Contracts.Abstracts.Account;
using FluentValidation;

namespace Application.Validations.Account
{
    public class VerifyTokenValidator : AbstractValidator<VerifyTokenRequest>
    {
        public VerifyTokenValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required!");
        }
    }
}
