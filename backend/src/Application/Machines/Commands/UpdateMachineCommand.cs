using MediatR;
using SentinelOps.Application.DTOs;
using SentinelOps.Domain.Entities;
using SentinelOps.Domain.Enums;
using SentinelOps.Domain.Repositories;
using SentinelOps.Shared.Models;

namespace SentinelOps.Application.Machines.Commands;

public record UpdateMachineCommand(
    int Id,
    string? Name,
    string? Code,
    string? SerialNumber,
    int? MachineTypeId,
    string? Model,
    string? Location,
    string? SpecificationsJson,
    string? QrCodeData,
    DateTime? InstallDate,
    Criticality? Criticality,
    MachineStatus? Status) : IRequest<Result<MachineDto>>;

public class UpdateMachineCommandHandler : IRequestHandler<UpdateMachineCommand, Result<MachineDto>>
{
    private readonly IMachineRepository _machineRepository;
    private readonly IMachineTypeRepository _machineTypeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMachineCommandHandler(IMachineRepository machineRepository, IMachineTypeRepository machineTypeRepository, IUnitOfWork unitOfWork)
    {
        _machineRepository = machineRepository;
        _machineTypeRepository = machineTypeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MachineDto>> Handle(UpdateMachineCommand request, CancellationToken cancellationToken)
    {
        var machine = await _machineRepository.GetByIdAsync(request.Id);
        if (machine is null)
        {
            return Result<MachineDto>.Failure("Machine not found.");
        }

        if (!string.IsNullOrWhiteSpace(request.Name)) machine.Name = request.Name.Trim();
        if (!string.IsNullOrWhiteSpace(request.Code)) machine.Code = request.Code.Trim();
        if (!string.IsNullOrWhiteSpace(request.SerialNumber)) machine.SerialNumber = request.SerialNumber.Trim();
        if (!string.IsNullOrWhiteSpace(request.Model)) machine.Model = request.Model.Trim();
        if (!string.IsNullOrWhiteSpace(request.Location)) machine.Location = request.Location.Trim();
        if (request.SpecificationsJson is not null) machine.SpecificationsJson = request.SpecificationsJson;
        if (request.QrCodeData is not null) machine.QrCodeData = request.QrCodeData;
        if (request.InstallDate.HasValue) machine.InstallDate = request.InstallDate.Value;
        if (request.Criticality.HasValue) machine.Criticality = request.Criticality.Value;
        if (request.Status.HasValue) machine.Status = request.Status.Value;

        if (request.MachineTypeId.HasValue)
        {
            var machineType = await _machineTypeRepository.GetByIdAsync(request.MachineTypeId.Value);
            if (machineType is null)
            {
                return Result<MachineDto>.Failure("Machine type not found.");
            }

            machine.MachineTypeId = request.MachineTypeId.Value;
        }

        machine.UpdatedAt = DateTime.UtcNow;
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
