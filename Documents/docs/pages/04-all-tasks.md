# All Tasks

## Purpose
Manage every task regardless of due date or completion.

## UI layout
Search/filter row, full task grid, pagination or virtualized loading as needed, action toolbar.

## Controls (Windows Forms)
`DataGridView`, `TextBox`, `ComboBox`, `Button`, `ContextMenuStrip`.

## User interactions
- Create, edit, delete, complete/reopen.
- Filter by category, priority, status and date.
- Sort by name, creation date and due date.

## Data and validation
Task fields: Id, Title, Description, DueAt, Priority, Status, CategoryId, CreatedAt, UpdatedAt, CompletedAt.

## Acceptance criteria
- All CRUD actions work and are validated.
- Filters combine predictably.
- No tasks disappear without a deliberate filter.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
