# Dashboard

## Purpose
Overview and rapid task management; this is the only page directly visible in the supplied screenshot.

## UI layout
Left navigation (about 220px), top action toolbar, central task grid, right quick-add panel and MonthCalendar (about 270px). Layout must resize gracefully.

## Controls (Windows Forms)
`Panel`, `ToolStrip` or `FlowLayoutPanel`, `DataGridView`, `TextBox` (search), `ComboBox`, `DateTimePicker`, `MonthCalendar`, `Button`, `CheckBox`. Grid columns: Completed, Task Name, Due Date, Priority, Category.

## User interactions
- New/Edit opens task editor; Delete requires confirmation.
- Mark Complete toggles selected task status.
- Search filters title/description; Filter & Sort applies status, category, priority, due date.
- Quick Add creates a task with title, optional description, due date and priority.
- Calendar day selection filters or highlights matching tasks.

## Data and validation
Quick Add requires a nonblank title; due date is optional in the domain even if the picker defaults to today. Priority values: Low, Medium, High, Critical. Grid binds to a view model; no direct Excel file I/O in event handlers.

## Acceptance criteria
- Grid loads tasks and displays correct status, due date, priority and category.
- CRUD, search, sorting, completion and quick-add persist after restart.
- Selection and empty-list states behave correctly.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
