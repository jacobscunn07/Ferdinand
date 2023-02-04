using FluentValidation;

namespace Ferdinand.Application.Queries.GetColor;

public class GetColorQueryValidator : AbstractValidator<GetColorQuery>
{
    public GetColorQueryValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .NotNull()
            .WithMessage("Must be a valid GUID");
    }
}
