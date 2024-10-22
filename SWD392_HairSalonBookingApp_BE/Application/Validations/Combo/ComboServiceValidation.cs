using Domain.Contracts.DTO.Combo;
using System;

namespace Application.Validations.Combo
{
    public static class ComboServiceValidation
    {
        public static void Validate(ComboServiceDTO comboServiceDTO)
        {
            if (string.IsNullOrEmpty(comboServiceDTO.ComboServiceName))
            {
                throw new ArgumentException("Combo service name is required");
            }
            if (comboServiceDTO.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero");
            }
            if (comboServiceDTO.SalonId == Guid.Empty)
            {
                throw new ArgumentException("SalonId is required");
            }
        }
    }
}
