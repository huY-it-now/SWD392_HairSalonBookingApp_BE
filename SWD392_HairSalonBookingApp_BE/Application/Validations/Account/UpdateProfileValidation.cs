using FluentValidation;

public class UpdateProfileValidation : AbstractValidator<UpdateProfileRequest> {
    public UpdateProfileValidation() {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email address is wrong format!");
    }
}