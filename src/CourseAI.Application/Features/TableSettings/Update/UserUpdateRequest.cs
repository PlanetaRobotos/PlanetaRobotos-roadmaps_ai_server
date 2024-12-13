using System.Text.Json.Serialization;
using CourseAI.Application.Core;
using CourseAI.Core.Enums;
using FluentValidation;
using NeerCore.Exceptions;

namespace CourseAI.Application.Features.TableSettings.Update;

public class UpdateTableSettingsRequest : IRequestModel
{
    [JsonIgnore]
    public long Id { get; set; }
    public string TableName { get; set; } = null!;
    public string[]? Columns { get; set; }

    [JsonIgnore]
    public TableSettingsName TableSettingsName =>
        Enum.TryParse<TableSettingsName>(TableName, out var val)
            ? val
            : throw new ValidationFailedException("Invalid table name");

    public class Validator : AbstractValidator<UpdateTableSettingsRequest>
    {
        public Validator()
        {
            RuleFor(x => x.TableName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Columns).NotEmpty()
                .ForEach(col => col.NotEmpty().MaximumLength(100));
        }
    }
}
