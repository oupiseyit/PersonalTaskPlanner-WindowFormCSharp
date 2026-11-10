# Reports

## Purpose
Review task completion and workload trends.

## UI layout
Date-range filter and summary cards, priority/category breakdown, daily completion chart and export button.

## Controls (Windows Forms)
`DateTimePicker`, `Label`, `DataGridView`, chart library (optional), `SaveFileDialog`.

## User interactions
- Choose date range.
- Display created, completed, overdue and open counts.
- Export CSV.

## Data and validation
Define completion rate as completed tasks / tasks created within selected period, and label denominator explicitly; separate completion-by-date metric if desired.

## Acceptance criteria
- Counts agree with Excel workbook records.
- Empty periods show zero values.
- CSV opens correctly in Excel.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
