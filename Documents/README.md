# Personal Task Planner — Windows Forms Build Specification

**Source:** user-provided screenshot of “Personal Task Planner - Pro Edition”.

**Goal:** Build a local-first C# Windows Forms task planner resembling the screenshot, with functional navigation, CRUD, scheduling, categories, reporting and settings.

## Documentation index
- [Architecture and UI Shell](docs/architecture.md)
- [Excel Workbook Storage Specification](docs/excel-storage.md)
- [Build Roadmap](docs/roadmap.md)
- [Dashboard](docs/pages/01-dashboard.md)
- [Calendar View](docs/pages/02-calendar-view.md)
- [Today's Plan](docs/pages/03-todays-plan.md)
- [All Tasks](docs/pages/04-all-tasks.md)
- [Categories](docs/pages/05-categories.md)
- [Reports](docs/pages/06-reports.md)
- [Settings](docs/pages/07-settings.md)
- [Task Editor](docs/pages/08-task-editor.md)

## Implementation order
Read architecture and Excel storage specification first, then follow roadmap phases. Build shared shell before individual pages. Use page acceptance criteria as implementation checkpoints.

## Screenshot scope
Only Dashboard is shown. Other pages and Task Editor are proposed designs based on navigation labels and expected task planner behavior, not literal screenshot reconstructions.

## Future extensions (out of MVP)
Recurring tasks, reminders with background scheduler, attachments, multi-user sync, account login, mobile companion.
