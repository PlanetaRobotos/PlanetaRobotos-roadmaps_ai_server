// using CourseAI.Application.Models.Jobs;
// using CourseAI.Application.Models.JobSettings;

namespace CourseAI.Application.Models.Shared;

public class FilteredBase<TModel>
{
    public TModel[] Data { get; init; } = null!;
    public long Total { get; init; }
}

public class Filtered<TModel> : FilteredBase<TModel>
{
    public string[]? Columns { get; set; }
}

// public class FilteredJobs : FilteredBase<JobModel>
// {
//     public string[]? Columns { get; set; }
//
//     /// <summary>
//     /// key: JobStatus
//     /// value: int
//     /// </summary>
//     public IReadOnlyDictionary<string, int> TotalByStatus { get; set; } = null!;
// }