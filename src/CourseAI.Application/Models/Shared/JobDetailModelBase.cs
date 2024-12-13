using CourseAI.Core.Constants;
using CourseAI.Core.Types;
using FluentValidation;

namespace CourseAI.Application.Models.Shared;

public abstract class JobDetailModelBase
{
    /// <example>ABC-4242</example>
    public string SKU { get; set; } = null!;

    /// <example>Product A</example>
    public string? Description { get; set; }

    /// <example>2</example>
    public int Quantity { get; set; }

    /// <example>
    /// {
    ///   "itemPoNumber": "000AB12C21"
    /// }
    /// </example>
    public FieldsDictionary? Fields { get; set; }


    protected static void ConfigureBaseValidator<TModel>(InlineValidator<TModel> validator)
        where TModel : JobDetailModelBase
    {
        validator.RuleFor(x => x.SKU).MaximumLength(StringLimits._100).NotEmpty();
        validator.RuleFor(x => x.Description).MaximumLength(StringLimits._500).NotEmpty();
        validator.RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
    }
}