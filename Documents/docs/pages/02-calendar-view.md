# Calendar View

## Purpose
Browse tasks by due date and inspect daily schedules.

## UI layout
Header with previous/next month, Today button, month calendar, and selected-day task list.

## Controls (Windows Forms)
`MonthCalendar`, `ListView` or `DataGridView`, `Button`, `Label`, `Panel`.

## User interactions
- Navigate months and select a date.
- Show tasks due that day, with priority/status.
- Double-click to edit; add task with selected due date.

## Data and validation
Use local date/time; distinguish tasks with no due date. Tasks spanning dates are not supported in MVP.

## Acceptance criteria
- Month navigation works.
- Selected day shows matching tasks only.
- Task edits update calendar immediately.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
