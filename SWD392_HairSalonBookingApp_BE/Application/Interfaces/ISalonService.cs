using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Salon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISalonService
    {
        Task<Result<object>> CreateSalon(CreateSalonDTO req);
    }
}
