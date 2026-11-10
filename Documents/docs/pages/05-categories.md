# Categories

## Purpose
Create and maintain task categories.

## UI layout
Category list/grid on left; add/edit fields on right or in modal.

## Controls (Windows Forms)
`DataGridView`, `TextBox`, `Button`, `ColorDialog` (optional).

## User interactions
- Add, rename, recolor and delete category.
- Show task count per category.
- Ask how to handle tasks before deleting a category.

## Data and validation
Category name required and unique case-insensitively. On deletion, reassign tasks to Uncategorized (null CategoryId) after confirmation.

## Acceptance criteria
- Duplicate names are rejected.
- Existing task associations survive rename.
- Delete behavior is predictable.

## Implementation notes
- Implement as a `UserControl` hosted in `MainForm.ContentPanel`, except modal editors.
- Use async service calls where appropriate; update UI on the UI thread.
- Provide keyboard navigation, clear empty states, confirmation for destructive actions, and friendly errors.
- Match the reference screenshot's dark navy navigation, light content area, blue selection and accent color.
