using Domain.Contracts.Abstracts.Salon;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validations.Salon
{
    public class CreateSalonRequestValidation : AbstractValidator<CreateSalonRequest>
    {
        public CreateSalonRequestValidation()
        {
            RuleFor(x => x.salonName).NotEmpty().WithMessage("Chi nhanh bat buoc");
            RuleFor(x => x.Address).NotEmpty().WithMessage("address is required!");
            RuleFor(x => x.Image).Must(BeAValidImage).WithMessage("File must be a valid image (jpg, jpeg, png) and less than or equal to 5MB.");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return false;

            if (file.Length > 5 * 1024 * 1024)
                return false;

            return true;
        }
    }
}
