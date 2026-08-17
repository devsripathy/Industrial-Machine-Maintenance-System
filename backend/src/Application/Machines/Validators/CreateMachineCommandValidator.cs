using FluentValidation;
using SentinelOps.Application.Machines.Commands;

namespace SentinelOps.Application.Machines.Validators;

public class CreateMachineCommandValidator : AbstractValidator<CreateMachineCommand>
{
    public CreateMachineCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.SerialNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .NotEmpty();

        RuleFor(x => x.Location)
            .NotEmpty();
    }
}
