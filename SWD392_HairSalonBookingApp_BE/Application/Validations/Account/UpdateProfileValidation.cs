using FluentValidation;

public class UpdateProfileValidation : AbstractValidator<UpdateProfileRequest> {
    public UpdateProfileValidation() {
        RuleFor(x => x.Email).EmailAddress().WithMessage("email address is wrong format");
    }
}