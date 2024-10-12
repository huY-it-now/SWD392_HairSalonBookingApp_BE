using Domain.Contracts.DTO.Combo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (comboDetailDTO.ComboServiceId == Guid.Empty)
            {
                throw new ArgumentException("ComboServiceId is required");
            }
        }
    }
}
