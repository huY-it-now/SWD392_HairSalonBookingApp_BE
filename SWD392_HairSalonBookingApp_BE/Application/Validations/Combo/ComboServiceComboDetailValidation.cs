using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Combo
{
    public static class ComboServiceComboDetailValidation
    {
        public static void Validate(Guid comboServiceId, Guid comboDetailId)
        {
            if (comboServiceId == Guid.Empty)
            {
                throw new ArgumentException("ComboServiceId is required");
            }

            if (comboDetailId == Guid.Empty)
            {
                throw new ArgumentException("ComboDetailId is required");
            }
        }
    }
}

