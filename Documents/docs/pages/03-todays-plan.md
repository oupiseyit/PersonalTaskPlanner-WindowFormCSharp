# Today's Plan

## Purpose
Focus on due-today and overdue incomplete tasks.

## UI layout
Summary strip (due today, overdue, completed today), task list, priority filter.

## Controls (Windows Forms)
`Label`, `DataGridView`, `ComboBox`, `Button`, `Panel`.

## User interactions
- Toggle completion.
- Sort by due time and priority.
- Open editor; optionally defer due date.

## Data and validation
Today means local calendar date; overdue means due before today or earlier today if due time is set and still incomplete.

## Acceptance criteria
- Counts match underlying data.
- Overdue tasks are visually distinct.
- Completion changes persist.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
