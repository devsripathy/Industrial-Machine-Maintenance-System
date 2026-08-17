using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.Machines.Queries;

public record GetMachinesQuery(string? SearchTerm = null, string? TypeCode = null, int? MinCriticality = null) : IRequest<Result<IEnumerable<MachineDto>>>;

public class GetMachinesQueryHandler : IRequestHandler<GetMachinesQuery, Result<IEnumerable<MachineDto>>>
{
    private readonly IMachineRepository _machineRepository;

    public GetMachinesQueryHandler(IMachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }

    public async Task<Result<IEnumerable<MachineDto>>> Handle(GetMachinesQuery request, CancellationToken cancellationToken)
    {
        var machines = await _machineRepository.GetAllAsync(request.SearchTerm, request.TypeCode, request.MinCriticality);
        var results = machines.OrderBy(m => m.Name).Select(MapToDto).ToList();
        return Result<IEnumerable<MachineDto>>.Success(results);
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
