using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Abstracts.Service;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Service;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IServiceService
    {
        Task<Result<object>> GetAllServices();
        Task<Result<object>> GetServiceById(Guid id);
        Task<Result<object>> CreateService(CreateServiceDTO request);
        Task<Result<object>> UpdateService(Guid id, UpdateServiceDTO request);
        Task<Result<object>> DeleteService(Guid id);
    }
}
