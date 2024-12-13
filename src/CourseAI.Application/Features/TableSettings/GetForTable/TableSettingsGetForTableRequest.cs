using CourseAI.Application.Core;
using CourseAI.Core.Enums;
using Newtonsoft.Json;

namespace CourseAI.Application.Features.TableSettings.GetForTable;

public class TableSettingsGetForTableRequest : IRequestModel<string[]>
{
    [JsonIgnore]
    public long UserId { get; init; }
    public TableSettingsName TableName { get; init; }
}
