namespace PersonalTaskPlanner.Domain;

public enum TaskPriority { Low, Medium, High, Critical }
public enum TaskStatus { Open, Completed }

public sealed class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset? DueAt { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public TaskStatus Status { get; set; } = TaskStatus.Open;
    public Guid? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}

public sealed class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = "#4F8EF7";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed record PlannerSettings(string StartupPage = "Dashboard", string DateFormat = "MM/dd/yyyy", bool RemindersEnabled = false);
public sealed record ReportMetrics(int Created, int Completed, int Overdue, int Open, double CompletionRate);
