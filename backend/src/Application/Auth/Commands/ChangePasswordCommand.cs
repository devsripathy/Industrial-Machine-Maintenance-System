using MediatR;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;
using SentinelOps.Shared.Utilities;

namespace SentinelOps.Application.Auth.Commands;

public record ChangePasswordCommand(int UserId, string CurrentPassword, string NewPassword) : IRequest<Result<bool>>;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0)
        {
            return Result<bool>.Failure("User not found.");
        }

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            return Result<bool>.Failure("User not found.");
        }

        if (string.IsNullOrWhiteSpace(request.CurrentPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return Result<bool>.Failure("Current and new passwords are required.");
        }

        if (!PasswordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
        {
            return Result<bool>.Failure("Current password is incorrect.");
        }

        if (request.NewPassword.Length < 8)
        {
            return Result<bool>.Failure("New password must be at least 8 characters long.");
        }

        user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
