# Settings

## Purpose
Configure application behavior and appearance.

## UI layout
Grouped panels: General, Notifications, Data, About.

## Controls (Windows Forms)
`CheckBox`, `ComboBox`, `Button`, `FolderBrowserDialog`, `Label`.

## User interactions
- Choose startup page and date display format.
- Enable optional reminder notifications.
- Back up the .xlsx workbook and restore after confirmation (validate sheet names and headers first).

## Data and validation
Store preferences separately from tasks. Validate backup path; warn that restore replaces local data. No cloud sync in MVP.

## Acceptance criteria
- Settings persist across restarts.
- Backup/restore round trip succeeds.
- Invalid paths show actionable errors.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
