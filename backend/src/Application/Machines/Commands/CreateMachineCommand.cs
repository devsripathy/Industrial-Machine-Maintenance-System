using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Enums;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.Machines.Commands;

public record CreateMachineCommand(
    string Name,
    string Code,
    string SerialNumber,
    int MachineTypeId,
    string Model,
    string Location,
    string? SpecificationsJson,
    string? QrCodeData,
    DateTime? InstallDate,
    Criticality Criticality,
    MachineStatus Status) : IRequest<Result<MachineDto>>;

public class CreateMachineCommandHandler : IRequestHandler<CreateMachineCommand, Result<MachineDto>>
{
    private readonly IMachineRepository _machineRepository;
    private readonly IMachineTypeRepository _machineTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateMachineCommandHandler(IMachineRepository machineRepository, IMachineTypeRepository machineTypeRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _machineTypeRepository = machineTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MachineDto>> Handle(CreateMachineCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.SerialNumber))
        {
            return Result<MachineDto>.Failure("Name, code, and serial number are required.");
        }

        var machineType = await _machineTypeRepository.GetByIdAsync(request.MachineTypeId);
        if (machineType is null)
        {
            return Result<MachineDto>.Failure("Machine type not found.");
        }

        if (await _machineRepository.GetByCodeAsync(request.Code.Trim()) is not null)
        {
            return Result<MachineDto>.Failure("A machine with this code already exists.");
        }

        var machine = new Machine
        {
            Name = request.Name.Trim(),
            Code = request.Code.Trim(),
            SerialNumber = request.SerialNumber.Trim(),
            MachineTypeId = request.MachineTypeId,
            Criticality = request.Criticality,
            Status = request.Status,
            Model = request.Model.Trim(),
            Location = request.Location.Trim(),
            SpecificationsJson = request.SpecificationsJson,
            QrCodeData = request.QrCodeData ?? $"sentinelops://machines/{request.Code.Trim()}",
            InstallDate = request.InstallDate ?? DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _machineRepository.AddAsync(machine);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<MachineDto>.Success(MapToDto(machine));
    }

    private static MachineDto MapToDto(Machine machine)
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
            Dependencies = new List<MachineDependencyDto>()
        };
    }
}
