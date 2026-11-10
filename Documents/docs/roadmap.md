# Build Roadmap and Definition of Done

## Phase 0 — Requirements and reference
- Review screenshot, page docs, workflow and data definitions.
- DoD: page behaviors and workbook schema approved.

## Phase 1 — Solution foundation
- Create WinForms solution and Domain/Application/Infrastructure/test projects.
- Configure DI, ClosedXML and logging.
- DoD: solution builds, app launches, automated test runner works.

## Phase 2 — Shared shell and visual system
- Implement MainForm sidebar, toolbar, navigation, responsive dock layout and common styles.
- DoD: all eight pages open without crashes; no empty broken controls.

## Phase 3 — Excel workbook storage and services
- Create workbook template and initialize Tasks, Categories, Settings sheets; implement ClosedXML repositories/services, seed data, validation, serialized writes and atomic file replacement.
- DoD: create/edit/delete/list tasks and categories persist after restart; tests pass.

## Phase 4 — Dashboard and Task Editor
- Implement grid, quick-add, search, actions and task dialog.
- DoD: end-to-end create → edit → complete → delete works.

## Phase 5 — All Tasks and Categories
- Add advanced filters, category CRUD and reassignment behavior.
- DoD: filters and category deletion pass acceptance tests.

## Phase 6 — Calendar and Today's Plan
- Implement date navigation, due-today/overdue logic.
- DoD: date boundary cases and completion refresh work.

## Phase 7 — Reports and Settings
- Implement metrics, CSV export, preferences and backup/restore.
- DoD: report totals reconcile with workbook data; backup restore validated.

## Phase 8 — QA and release
- Test UI at 1366×768 and 1920×1080, DPI, keyboard, invalid inputs, empty data and restart.
- Publish self-contained Windows x64 installer or folder build.
- DoD: clean installation, zero critical defects, user guide ready.

## Suggested test cases
- Empty title rejected; 200-char title accepted; duplicate category rejected.
- Task with no due date displays correctly.
- Mark complete/reopen updates CompletedAt and reports.
- Overdue tasks calculated correctly across midnight.
- Deleting a category leaves its tasks Uncategorized.
- Search and multiple filters compose correctly.
- Backup and restore round-trip data exactly.
