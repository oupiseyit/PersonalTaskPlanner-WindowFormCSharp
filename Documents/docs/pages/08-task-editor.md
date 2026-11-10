# Task Editor

## Purpose
Create or update a task with complete details.

## UI layout
Modal dialog with title, description, category, priority, due date/time, status and Save/Cancel.

## Controls (Windows Forms)
`Form`, `TextBox`, `RichTextBox`, `ComboBox`, `DateTimePicker`, `CheckBox`, `Button`, `ErrorProvider`.

## User interactions
- Save validates then creates/updates.
- Cancel discards unsaved changes.
- Optional due date checkbox enables date/time input.

## Data and validation
Title 1–200 characters; description optional; priority required; status Open/Completed; category optional; due date optional. CompletedAt set/cleared when status changes.

## Acceptance criteria
- Both create and edit paths work.
- Cancel never persists.
- Invalid title cannot save.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
