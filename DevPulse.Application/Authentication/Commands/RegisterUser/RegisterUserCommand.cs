using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Authentication.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Username,
    string Password
): IRequest<AuthResponse>;

public record AuthResponse(
    string Id,
    string Username,
    string Email,
    string Token,
    DateTime Expiration
);

public class RegisterUserCommandHandler: IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(UserManager<IdentityUser> userManager, IConfiguration configuration, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if(existingUser != null)
        {
            throw new Exception("An account with this email address already exists.");
        }

        var newUser = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Username,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(newUser,request.Password);
        if(!result.Succeeded)
        {
            var errors = string.Join(",", result.Errors.Select(e => e.Description));
            throw new Exception($"Account registration failed: {errors}");
        }

        var tokenData = _jwtTokenGenerator.GenerateToken(newUser);

        return new AuthResponse(
            newUser.Id,
            newUser.UserName,
            newUser.Email,
            tokenData.Token,
            tokenData.Expiration
        );
    }

    
}