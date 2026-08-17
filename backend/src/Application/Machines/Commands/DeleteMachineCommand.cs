using MediatR;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.Machines.Commands;

public record DeleteMachineCommand(int Id) : IRequest<Result<bool>>;

public class DeleteMachineCommandHandler : IRequestHandler<DeleteMachineCommand, Result<bool>>
{
    private readonly IMachineRepository _machineRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMachineCommandHandler(IMachineRepository machineRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteMachineCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return Result<bool>.Failure("Machine id must be greater than zero.");
        }

        var machine = await _machineRepository.GetByIdAsync(request.Id);
        if (machine is null)
        {
            return Result<bool>.Failure("Machine not found.");
        }

        _machineRepository.Delete(machine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
