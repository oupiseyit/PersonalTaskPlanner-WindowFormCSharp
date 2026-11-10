# Architecture and UI Shell

## Target stack
- C# / .NET 8 Windows Forms (Windows only)
- Excel .xlsx workbook via ClosedXML (NuGet); no SQL workbook, EF Core, or migrations
- Dependency injection with Microsoft.Extensions.DependencyInjection
- Local-first, single-user MVP; no authentication or cloud sync

## Project layout
```text
PersonalTaskPlanner.sln
src/
  PersonalTaskPlanner.WinForms/
    Forms/MainForm.cs
    Forms/TaskEditorForm.cs
    Controls/DashboardControl.cs
    Controls/CalendarViewControl.cs
    Controls/TodaysPlanControl.cs
    Controls/AllTasksControl.cs
    Controls/CategoriesControl.cs
    Controls/ReportsControl.cs
    Controls/SettingsControl.cs
  PersonalTaskPlanner.Application/
    Interfaces/ITaskService.cs
    Services/TaskService.cs
    DTOs/
  PersonalTaskPlanner.Domain/
    Entities/TaskItem.cs
    Entities/Category.cs
    Enums/TaskPriority.cs
  PersonalTaskPlanner.Infrastructure/
    Storage/ExcelWorkbookStore.cs
    Repositories/ExcelTaskRepository.cs
    Repositories/ExcelCategoryRepository.cs
tests/PersonalTaskPlanner.Tests/
docs/pages/
```

## MainForm shell
- Borderless custom title bar is optional; prefer standard window chrome for the MVP.
- Dock sidebar to Left, quick-add sidebar to Right only on Dashboard, and central page container to Fill.
- Navigation buttons replace the current UserControl, not create a new main window.
- Keep common task actions in application services, not form code.
- Use `BindingSource` for grid refresh and consistent selection behavior.

## Design reference
The screenshot shows a Windows 11-like desktop with dark navy left navigation, light gray/white main area, blue selected row and primary button, dense task table, and right quick-add/calendar column. Several visible sample labels and dates appear malformed; treat them as mock data, not product requirements. Other pages are inferred, not directly depicted.

## Nonfunctional requirements
- Usable at 1366×768 and larger; handle high DPI and font scaling.
- Keyboard access, visible focus and accessible labels.
- Workbook reads/writes must not freeze the UI; use async service calls and a serialized write queue.
- Use ClosedXML for workbook read/write; validate records, lock writes, and save atomically using a temporary file and replace.
- Log unexpected errors without exposing stack traces to end users.
