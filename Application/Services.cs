using System.Text;
using Microsoft.Extensions.Logging;
using PersonalTaskPlanner.Domain;

namespace PersonalTaskPlanner.Application;

public sealed class TaskService(IPlannerStore store, ILogger<TaskService> logger) : ITaskService
{
    public async Task<IReadOnlyList<TaskItem>> GetTasksAsync(CancellationToken cancellationToken = default) => (await store.ReadAsync(cancellationToken)).Tasks;
    public async Task<TaskItem> SaveAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        _ = logger;
        if (string.IsNullOrWhiteSpace(task.Title) || task.Title.Length > 200) throw new ArgumentException("Title must contain 1-200 characters.");
        var data = await store.ReadAsync(cancellationToken); var existing = data.Tasks.FirstOrDefault(x => x.Id == task.Id);
        task.UpdatedAt = DateTime.UtcNow;
        if (task.Status == PersonalTaskPlanner.Domain.TaskStatus.Completed && existing?.Status != PersonalTaskPlanner.Domain.TaskStatus.Completed) task.CompletedAt = DateTime.UtcNow;
        if (task.Status == PersonalTaskPlanner.Domain.TaskStatus.Open) task.CompletedAt = null;
        if (existing is null) { task.CreatedAt = DateTime.UtcNow; data.Tasks.Add(task); } else { data.Tasks[data.Tasks.IndexOf(existing)] = task; }
        await store.WriteAsync(data, cancellationToken); return task;
    }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) { var data = await store.ReadAsync(cancellationToken); data.Tasks.RemoveAll(x => x.Id == id); await store.WriteAsync(data, cancellationToken); }
    public async Task ToggleCompletedAsync(Guid id, CancellationToken cancellationToken = default) { var data = await store.ReadAsync(cancellationToken); var task = data.Tasks.FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException(); task.Status = task.Status == PersonalTaskPlanner.Domain.TaskStatus.Completed ? PersonalTaskPlanner.Domain.TaskStatus.Open : PersonalTaskPlanner.Domain.TaskStatus.Completed; task.CompletedAt = task.Status == PersonalTaskPlanner.Domain.TaskStatus.Completed ? DateTime.UtcNow : null; task.UpdatedAt = DateTime.UtcNow; await store.WriteAsync(data, cancellationToken); }
}

public sealed class CategoryService(IPlannerStore store) : ICategoryService
{
    public async Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken cancellationToken = default) => (await store.ReadAsync(cancellationToken)).Categories;
    public async Task<Category> SaveAsync(Category category, CancellationToken cancellationToken = default) { if (string.IsNullOrWhiteSpace(category.Name)) throw new ArgumentException("Category name is required."); var data = await store.ReadAsync(cancellationToken); if (data.Categories.Any(x => x.Id != category.Id && string.Equals(x.Name, category.Name, StringComparison.OrdinalIgnoreCase))) throw new ArgumentException("Category names must be unique."); var old = data.Categories.FirstOrDefault(x => x.Id == category.Id); if (old is null) data.Categories.Add(category); else data.Categories[data.Categories.IndexOf(old)] = category; await store.WriteAsync(data, cancellationToken); return category; }
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default) { var data = await store.ReadAsync(cancellationToken); data.Categories.RemoveAll(x => x.Id == id); foreach (var task in data.Tasks.Where(x => x.CategoryId == id)) task.CategoryId = null; await store.WriteAsync(data, cancellationToken); }
}

public sealed class SettingsService(IPlannerStore store) : ISettingsService
{
    public async Task<PlannerSettings> GetAsync(CancellationToken cancellationToken = default) { var s = (await store.ReadAsync(cancellationToken)).Settings; return new PlannerSettings(s.GetValueOrDefault("StartupPage", "Dashboard"), s.GetValueOrDefault("DateFormat", "MM/dd/yyyy"), bool.TryParse(s.GetValueOrDefault("RemindersEnabled", "false"), out var r) && r); }
    public async Task SaveAsync(PlannerSettings settings, CancellationToken cancellationToken = default) { var d = await store.ReadAsync(cancellationToken); d.Settings["StartupPage"] = settings.StartupPage; d.Settings["DateFormat"] = settings.DateFormat; d.Settings["RemindersEnabled"] = settings.RemindersEnabled.ToString(); await store.WriteAsync(d, cancellationToken); }
    public Task BackupAsync(string destination, CancellationToken cancellationToken = default) => store.BackupAsync(destination, cancellationToken);
    public Task RestoreAsync(string source, CancellationToken cancellationToken = default) => store.RestoreAsync(source, cancellationToken);
}

public sealed class ReportService(IPlannerStore store) : IReportService
{
    public async Task<ReportMetrics> GetMetricsAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default) { var tasks = (await store.ReadAsync(cancellationToken)).Tasks; var local = tasks.Where(x => x.CreatedAt.ToLocalTime().Date >= start.Date && x.CreatedAt.ToLocalTime().Date <= end.Date).ToList(); var completed = local.Count(x => x.Status == PersonalTaskPlanner.Domain.TaskStatus.Completed); var overdue = tasks.Count(x => x.Status == Domain.TaskStatus.Open && x.DueAt is not null && x.DueAt.Value.LocalDateTime < DateTime.Now); return new ReportMetrics(local.Count, completed, overdue, tasks.Count(x => x.Status == Domain.TaskStatus.Open), local.Count == 0 ? 0 : completed * 100d / local.Count); }
    public async Task ExportCsvAsync(string destination, DateTime start, DateTime end, CancellationToken cancellationToken = default) { var tasks = (await store.ReadAsync(cancellationToken)).Tasks.Where(x => x.CreatedAt.ToLocalTime().Date >= start.Date && x.CreatedAt.ToLocalTime().Date <= end.Date); var sb = new StringBuilder("Id,Title,DueAt,Priority,Status\n"); foreach (var t in tasks) sb.AppendLine($"{t.Id},\"{t.Title.Replace("\"", "\"\"")}\",{t.DueAt:O},{t.Priority},{t.Status}"); await File.WriteAllTextAsync(destination, sb.ToString(), cancellationToken); }
}
