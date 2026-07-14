using System.Collections.Generic;
using SentinelOps.Domain.Entities;

namespace SentinelOps.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
}
