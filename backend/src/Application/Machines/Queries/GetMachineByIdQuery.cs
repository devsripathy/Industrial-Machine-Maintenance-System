using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.Machines.Queries;

public record GetMachineByIdQuery(int Id) : IRequest<Result<MachineDto>>;

public class GetMachineByIdQueryHandler : IRequestHandler<GetMachineByIdQuery, Result<MachineDto>>
{
    private readonly IMachineRepository _machineRepository;

    public GetMachineByIdQueryHandler(IMachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }

    public async Task<Result<MachineDto>> Handle(GetMachineByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return Result<MachineDto>.Failure("Machine id must be greater than zero.");
        }

        var machine = await _machineRepository.GetByIdAsync(request.Id);
        if (machine is null)
        {
            return Result<MachineDto>.Failure("Machine not found.");
        }

        return Result<MachineDto>.Success(MapToDto(machine));
    }

    private static MachineDto MapToDto(Domain.Entities.Machine machine)
    {
        return new MachineDto
        {
            Id = machine.Id,
            Name = machine.Name,
            Code = machine.Code,
            SerialNumber = machine.SerialNumber,
            MachineTypeId = machine.MachineTypeId,
            MachineTypeCode = machine.MachineType?.Code ?? string.Empty,
            MachineTypeName = machine.MachineType?.Name ?? string.Empty,
            Criticality = machine.Criticality.ToString(),
            CriticalityValue = (int)machine.Criticality,
            Status = machine.Status.ToString(),
            StatusValue = (int)machine.Status,
            Model = machine.Model,
            Location = machine.Location,
            SpecificationsJson = machine.SpecificationsJson,
            QrCodeData = machine.QrCodeData,
            InstallDate = machine.InstallDate,
            CreatedAt = machine.CreatedAt,
            UpdatedAt = machine.UpdatedAt,
            Dependencies = machine.ParentDependencies.Select(d => new MachineDependencyDto
            {
                MachineId = d.MachineId,
                DependsOnMachineId = d.DependsOnMachineId,
                DependsOnMachineName = d.DependsOnMachine?.Name ?? string.Empty,
                DependsOnMachineCode = d.DependsOnMachine?.Code ?? string.Empty,
                DependencyType = d.DependencyType ?? "Process"
            }).ToList()
        };
    }
}
