using System.Text.RegularExpressions;
using FluentValidation;

namespace Ferdinand.Application.Queries.FindColors;

public class FindColorsQueryValidator : AbstractValidator<FindColorsQuery>
{
    public FindColorsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
            .MaximumLength(6)
            .Matches(new Regex(@"^[0-9a-fA-F]{0,6}$"))
            .WithMessage("Search term must be a hexadecimal number with a maximum length of 6");
    }
}
