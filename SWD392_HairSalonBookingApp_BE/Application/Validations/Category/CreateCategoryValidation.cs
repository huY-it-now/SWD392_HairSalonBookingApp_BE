using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Abstracts.Category;
using FluentValidation;

namespace Application.Validations.Category
{
    public class CreateCategoryValidation : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryValidation()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required");
        }
    }
}
