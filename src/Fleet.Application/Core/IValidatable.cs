using FluentValidation;

namespace Fleet.Application.Core;

public interface IValidatable<TModel>
{
    public void ConfigureValidator(InlineValidator<TModel> validator);
}