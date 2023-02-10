using System.Text.RegularExpressions;
using FluentValidation;

namespace Ferdinand.Application.Commands.AddColor;

public class AddColorCommandValidator : AbstractValidator<AddColorCommand>
{
    public AddColorCommandValidator()
    {
        RuleFor(x => x.Tenant)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.HexValue)
            .Matches(new Regex(@"^[0-9a-fA-F]{6}$"))
            .WithMessage("Hex color code must be a valid hex color code");
    }
}
