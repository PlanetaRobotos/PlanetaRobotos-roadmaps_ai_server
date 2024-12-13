using Fleet.Application.Core;
using FluentValidation;
using Sieve.Models;

namespace Fleet.Application.Models.Shared;

public abstract class FilterRequestBase<TModel> : SieveModel, IValidatable<TModel>
    where TModel : SieveModel
{
    public string? Search { get; init; }
    public bool? IncludeColumns { get; init; }

    public void ConfigureValidator(InlineValidator<TModel> validator)
    {
        validator.RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        validator.RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
    }
}