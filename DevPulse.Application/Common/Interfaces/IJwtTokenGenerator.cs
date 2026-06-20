using Microsoft.AspNetCore.Identity;

namespace DevPulse.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime Expiration) GenerateToken(IdentityUser user);
}