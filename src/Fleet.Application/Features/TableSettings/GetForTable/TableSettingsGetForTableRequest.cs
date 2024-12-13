using Fleet.Application.Core;
using Fleet.Core.Enums;
using Newtonsoft.Json;

namespace Fleet.Application.Features.TableSettings.GetForTable;

public class TableSettingsGetForTableRequest : IRequestModel<string[]>
{
    [JsonIgnore]
    public long UserId { get; init; }
    public TableSettingsName TableName { get; init; }
}
