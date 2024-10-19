using FluentValidation;

public class ViewSalonRequestValidator : AbstractValidator<ViewSalonRequest> {
    public ViewSalonRequestValidator() {
        RuleFor(x => x.SalonId).NotEmpty().WithMessage("salon id is required");
    }
}