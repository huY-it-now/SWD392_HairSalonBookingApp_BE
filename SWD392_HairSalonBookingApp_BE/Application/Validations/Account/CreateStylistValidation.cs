using FluentValidation;

public class CreateStylistRequestValidation : AbstractValidator<CreateStylistRequest>
{
    public CreateStylistRequestValidation()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User is required!");

        RuleFor(x => x.SalonId)
            .NotEmpty()
            .WithMessage("Salon is required!");

        RuleFor(x => x.Job)
            .NotEmpty()
            .WithMessage("Job is required!");
    }
}