using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DevPulse.Application.Authentication.Commands.RegisterUser;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Authentication.Queries.LoginUser;

// 1. THE CONTRACT (The Payload)
public record LoginUserQuery(string Username, string Password) : IRequest<AuthResponse>;

// 2. THE LOGIC (The Execution Engine)
public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, AuthResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserQueryHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null) throw new Exception("Invalid username or password credentials.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid) throw new Exception("Invalid username or password credentials.");

        var tokenData = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResponse(
            user.Id,
            user.UserName!,
            user.Email!,
            tokenData.Token,
            tokenData.Expiration
        );
    }

    
}