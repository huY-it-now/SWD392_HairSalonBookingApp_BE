using Domain.Contracts.DTO.Combo;
using System;

namespace Application.Validations.Combo
{
    public static class ComboDetailValidation
    {
        public static void Validate(ComboDetailDTO comboDetailDTO)
        {
            if (string.IsNullOrEmpty(comboDetailDTO.Content))
            {
                throw new ArgumentException("Content is required");
            }
        }
    }
}
