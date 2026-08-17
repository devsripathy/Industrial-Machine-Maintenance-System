using MediatR;
using SentinelOps.Application.Common.Interfaces;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;
using SentinelOps.Shared.Utilities;

namespace SentinelOps.Application.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result<AuthResponseDto>>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var password = request.Password;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return Result<AuthResponseDto>.Failure("Username and password are required.");
        }

        var user = await _userRepository.GetByUsernameAsync(username)
            ?? await _userRepository.GetByEmailAsync(username);

        if (user is null || !user.IsActive || !PasswordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return Result<AuthResponseDto>.Failure("Invalid username or password.");
        }

        var permissions = user.Role.Permissions.Select(p => p.Name).ToList();
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user, permissions);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AuthResponseDto>.Success(new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role.Name
            }
        });
    }
}
