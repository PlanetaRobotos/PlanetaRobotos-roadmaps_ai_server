using FluentValidation;

namespace CourseAI.Application.Core;

public interface IValidatable<TModel>
{
    public void ConfigureValidator(InlineValidator<TModel> validator);
}