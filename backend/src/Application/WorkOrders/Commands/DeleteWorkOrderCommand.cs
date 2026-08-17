using MediatR;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.WorkOrders.Commands;

public record DeleteWorkOrderCommand(int Id) : IRequest<Result<bool>>;

public class DeleteWorkOrderCommandHandler : IRequestHandler<DeleteWorkOrderCommand, Result<bool>>
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkOrderCommandHandler(IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return Result<bool>.Failure("Work order id must be greater than zero.");
        }

        var workOrder = await _workOrderRepository.GetByIdAsync(request.Id);
        if (workOrder is null)
        {
            return Result<bool>.Failure("Work order not found.");
        }

        _workOrderRepository.Delete(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
