// using CourseAI.Application.Models.JobSettings;
using CourseAI.Core.Enums;

namespace CourseAI.Application.Services;

public interface ITableSettingsService
{
    Task<string[]> GetColumnsAsync<TTable>(long organizationId, long userId, TableSettingsName tableName, CancellationToken ct = default);
    Task<string[]> GetAllColumnsAsync<TTable>(TableSettingsName tableName, CancellationToken ct = default);
    // Task<JobFieldSettingsModel[]> GetJobColumnsAsync(long organizationId, long userId, TableSettingsName tableName, CancellationToken ct = default);
    // Task<JobFieldSettingsModel[]> GetAllJobsColumnsAsync(long organizationId, TableSettingsName tableName, CancellationToken ct = default);
}