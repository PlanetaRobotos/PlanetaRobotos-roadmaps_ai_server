// using Fleet.Application.Models.JobSettings;
using Fleet.Core.Enums;

namespace Fleet.Application.Services;

public interface ITableSettingsService
{
    Task<string[]> GetColumnsAsync<TTable>(long organizationId, long userId, TableSettingsName tableName, CancellationToken ct = default);
    Task<string[]> GetAllColumnsAsync<TTable>(TableSettingsName tableName, CancellationToken ct = default);
    // Task<JobFieldSettingsModel[]> GetJobColumnsAsync(long organizationId, long userId, TableSettingsName tableName, CancellationToken ct = default);
    // Task<JobFieldSettingsModel[]> GetAllJobsColumnsAsync(long organizationId, TableSettingsName tableName, CancellationToken ct = default);
}