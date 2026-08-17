using FluentValidation;
using SentinelOps.Application.Machines.Commands;

namespace SentinelOps.Application.Machines.Validators;

public class UpdateMachineCommandValidator : AbstractValidator<UpdateMachineCommand>
{
    public UpdateMachineCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Code)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.SerialNumber));
    }
}
