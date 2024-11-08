using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IJWTService
    {
        string GenerateToken(Guid userId, Guid sessionId, string email, UserStatusEnum status, bool isSuperAdmin, int exp);
    }
}
