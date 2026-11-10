using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using PersonalTaskPlanner.Application;
using PersonalTaskPlanner.Domain;
using System.Collections.Concurrent;
using System.Globalization;

namespace PersonalTaskPlanner.Infrastructure;

public sealed class ExcelWorkbookStore : IPlannerStore
{
    private static readonly string[] TaskHeaders = { "Id", "Title", "Description", "DueAt", "Priority", "Status", "CategoryId", "CreatedAt", "UpdatedAt", "CompletedAt" };
    private static readonly string[] CategoryHeaders = { "Id", "Name", "ColorHex", "CreatedAt" };
    private static readonly string[] SettingHeaders = { "Key", "Value" };
    private readonly ILogger<ExcelWorkbookStore> _logger;
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private readonly string _path;
    private bool _initialized;

    public ExcelWorkbookStore(ILogger<ExcelWorkbookStore> logger)
    {
        _logger = logger;
        _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersonalTaskPlanner", "planner-data.xlsx");
    }

    public string FilePath => _path;

    public async Task<PlannerData> ReadAsync(CancellationToken cancellationToken = default)
    {
        await EnsureInitializedAsync(cancellationToken);
        try
        {
            using var workbook = new XLWorkbook(_path);
            ValidateSchema(workbook);
            return ReadData(workbook);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
        {
            _logger.LogError(ex, "Unable to read planner workbook at {Path}", _path);
            throw new InvalidDataException("The planner workbook is locked, corrupt, or inaccessible. Use Settings to restore a valid backup.", ex);
        }
    }

    public async Task WriteAsync(PlannerData data, CancellationToken cancellationToken = default)
    {
        ValidateData(data);
        await EnsureInitializedAsync(cancellationToken);
        await _writeLock.WaitAsync(cancellationToken);
        try
        {
            var directory = Path.GetDirectoryName(_path)!;
            var temp = Path.Combine(directory, $"planner-data.{Guid.NewGuid():N}.tmp.xlsx");
            using (var workbook = new XLWorkbook())
            {
                WriteData(workbook, data);
                workbook.SaveAs(temp);
            }
            var backup = _path + ".bak";
            if (File.Exists(_path)) File.Copy(_path, backup, true);
            File.Move(temp, _path, true);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Unable to write planner workbook at {Path}", _path);
            throw new IOException("The planner workbook could not be saved. Close it in Excel and try again.", ex);
        }
        finally { _writeLock.Release(); }
    }

    public async Task BackupAsync(string destination, CancellationToken cancellationToken = default)
    {
        var data = await ReadAsync(cancellationToken);
        var target = Path.GetFullPath(destination);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        await _writeLock.WaitAsync(cancellationToken);
        try { using var workbook = new XLWorkbook(); WriteData(workbook, data); workbook.SaveAs(target); }
        finally { _writeLock.Release(); }
    }

    public async Task RestoreAsync(string source, CancellationToken cancellationToken = default)
    {
        var candidate = Path.GetFullPath(source);
        if (!File.Exists(candidate)) throw new FileNotFoundException("The selected backup file does not exist.", candidate);
        using (var workbook = new XLWorkbook(candidate)) { ValidateSchema(workbook); }
        await _writeLock.WaitAsync(cancellationToken);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            if (File.Exists(_path)) File.Copy(_path, _path + ".restore-bak", true);
            File.Copy(candidate, _path, true);
        }
        finally { _writeLock.Release(); }
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        if (_initialized && File.Exists(_path)) return;
        await _writeLock.WaitAsync(cancellationToken);
        try
        {
            if (File.Exists(_path)) { _initialized = true; return; }
            Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
            using var workbook = new XLWorkbook();
            WriteData(workbook, new PlannerData());
            workbook.SaveAs(_path);
            _initialized = true;
        }
        finally { _writeLock.Release(); }
    }

    private static PlannerData ReadData(XLWorkbook workbook)
    {
        var result = new PlannerData();
        foreach (var row in workbook.Worksheet("Tasks").RowsUsed().Skip(1))
        {
            if (row.Cell(1).IsEmpty()) continue;
            result.Tasks.Add(new TaskItem { Id = Guid.Parse(row.Cell(1).GetString()), Title = row.Cell(2).GetString(), Description = row.Cell(3).GetString(), DueAt = ParseOffset(row.Cell(4).GetString()), Priority = Enum.Parse<TaskPriority>(row.Cell(5).GetString()), Status = Enum.Parse<PersonalTaskPlanner.Domain.TaskStatus>(row.Cell(6).GetString()), CategoryId = ParseGuid(row.Cell(7).GetString()), CreatedAt = ParseUtc(row.Cell(8).GetString()), UpdatedAt = ParseUtc(row.Cell(9).GetString()), CompletedAt = ParseNullableUtc(row.Cell(10).GetString()) });
        }
        foreach (var row in workbook.Worksheet("Categories").RowsUsed().Skip(1))
        {
            if (row.Cell(1).IsEmpty()) continue;
            result.Categories.Add(new Category { Id = Guid.Parse(row.Cell(1).GetString()), Name = row.Cell(2).GetString(), ColorHex = row.Cell(3).GetString(), CreatedAt = ParseUtc(row.Cell(4).GetString()) });
        }
        foreach (var row in workbook.Worksheet("Settings").RowsUsed().Skip(1)) if (!row.Cell(1).IsEmpty()) result.Settings[row.Cell(1).GetString()] = row.Cell(2).GetString();
        return result;
    }

    private static void WriteData(XLWorkbook workbook, PlannerData data)
    {
        var tasks = workbook.Worksheets.Add("Tasks"); WriteHeaders(tasks, TaskHeaders);
        for (var i = 0; i < data.Tasks.Count; i++) { var t = data.Tasks[i]; var r = i + 2; tasks.Cell(r, 1).Value = t.Id.ToString(); tasks.Cell(r, 2).Value = t.Title; tasks.Cell(r, 3).Value = t.Description; tasks.Cell(r, 4).Value = t.DueAt?.ToString("O") ?? ""; tasks.Cell(r, 5).Value = t.Priority.ToString(); tasks.Cell(r, 6).Value = t.Status.ToString(); tasks.Cell(r, 7).Value = t.CategoryId?.ToString() ?? ""; tasks.Cell(r, 8).Value = t.CreatedAt.ToUniversalTime().ToString("O"); tasks.Cell(r, 9).Value = t.UpdatedAt.ToUniversalTime().ToString("O"); tasks.Cell(r, 10).Value = t.CompletedAt?.ToUniversalTime().ToString("O") ?? ""; }
        var categories = workbook.Worksheets.Add("Categories"); WriteHeaders(categories, CategoryHeaders);
        for (var i = 0; i < data.Categories.Count; i++) { var c = data.Categories[i]; var r = i + 2; categories.Cell(r, 1).Value = c.Id.ToString(); categories.Cell(r, 2).Value = c.Name; categories.Cell(r, 3).Value = c.ColorHex; categories.Cell(r, 4).Value = c.CreatedAt.ToUniversalTime().ToString("O"); }
        var settings = workbook.Worksheets.Add("Settings"); WriteHeaders(settings, SettingHeaders); var s = 2; foreach (var pair in data.Settings) { settings.Cell(s, 1).Value = pair.Key; settings.Cell(s++, 2).Value = pair.Value; }
        foreach (var sheet in workbook.Worksheets) { sheet.Row(1).Style.Font.Bold = true; sheet.Columns().AdjustToContents(); }
    }
    private static void WriteHeaders(IXLWorksheet sheet, string[] headers) { for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i]; }
    private static void ValidateSchema(XLWorkbook workbook) { foreach (var pair in new[] { ("Tasks", TaskHeaders), ("Categories", CategoryHeaders), ("Settings", SettingHeaders) }) { if (!workbook.Worksheets.Contains(pair.Item1)) throw new InvalidDataException($"Workbook is missing the {pair.Item1} sheet."); var row = workbook.Worksheet(pair.Item1).Row(1); for (var i = 0; i < pair.Item2.Length; i++) if (!string.Equals(row.Cell(i + 1).GetString(), pair.Item2[i], StringComparison.Ordinal)) throw new InvalidDataException($"Workbook schema for {pair.Item1} is invalid."); } }
    private static void ValidateData(PlannerData data) { if (data.Tasks.GroupBy(x => x.Id).Any(x => x.Count() > 1) || data.Categories.GroupBy(x => x.Id).Any(x => x.Count() > 1)) throw new InvalidDataException("Duplicate IDs are not allowed."); if (data.Categories.GroupBy(x => x.Name, StringComparer.OrdinalIgnoreCase).Any(x => x.Count() > 1)) throw new InvalidDataException("Category names must be unique."); foreach (var t in data.Tasks) if (string.IsNullOrWhiteSpace(t.Title) || t.Title.Length > 200) throw new InvalidDataException("Task titles must contain 1-200 characters."); }
    private static Guid? ParseGuid(string value) => Guid.TryParse(value, out var result) ? result : null;
    private static DateTimeOffset? ParseOffset(string value) => DateTimeOffset.TryParse(value, null, DateTimeStyles.RoundtripKind, out var result) ? result : null;
    private static DateTime ParseUtc(string value) => DateTime.Parse(value, null, DateTimeStyles.RoundtripKind).ToUniversalTime();
    private static DateTime? ParseNullableUtc(string value) => string.IsNullOrWhiteSpace(value) ? null : ParseUtc(value);
}
