using PersonalTaskPlanner.Domain;

namespace PersonalTaskPlanner.Application;

public interface ITaskService
{
    Task<IReadOnlyList<TaskItem>> GetTasksAsync(CancellationToken cancellationToken = default);
    Task<TaskItem> SaveAsync(TaskItem task, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task ToggleCompletedAsync(Guid id, CancellationToken cancellationToken = default);
}
public interface ICategoryService
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<Category> SaveAsync(Category category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
public interface ISettingsService
{
    Task<PlannerSettings> GetAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(PlannerSettings settings, CancellationToken cancellationToken = default);
    Task BackupAsync(string destination, CancellationToken cancellationToken = default);
    Task RestoreAsync(string source, CancellationToken cancellationToken = default);
}
public interface IReportService
{
    Task<ReportMetrics> GetMetricsAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default);
    Task ExportCsvAsync(string destination, DateTime start, DateTime end, CancellationToken cancellationToken = default);
}
public interface IPlannerStore
{
    Task<PlannerData> ReadAsync(CancellationToken cancellationToken = default);
    Task WriteAsync(PlannerData data, CancellationToken cancellationToken = default);
    Task BackupAsync(string destination, CancellationToken cancellationToken = default);
    Task RestoreAsync(string source, CancellationToken cancellationToken = default);
}
public sealed class PlannerData
{
    public List<TaskItem> Tasks { get; init; } = new();
    public List<Category> Categories { get; init; } = new();
    public Dictionary<string, string> Settings { get; init; } = new(StringComparer.OrdinalIgnoreCase);
}
