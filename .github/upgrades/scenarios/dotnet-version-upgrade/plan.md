# Personal Task Planner .NET 8 Implementation Plan

## Overview

**Target**: Convert the legacy .NET Framework 4.7.2 WinForms planner into a .NET 8 Windows Forms application and implement the complete documented MVP.
**Scope**: One legacy desktop project restructured into Domain, Application, Infrastructure, WinForms, and tests, with ClosedXML workbook persistence and all documented pages.

## Upgrade Options

| Option | Selected | Why |
|--------|----------|-----|
| Target Framework | net8.0-windows | Explicit user requirement and documented target stack. |
| Storage | ClosedXML single workbook | Required by the storage specification; no database. |
| Execution Mode | Automatic | User requested full implementation without intermediate approval. |
| Architecture | Domain/Application/Infrastructure/WinForms/tests | Required by the architecture document and roadmap. |

## Tasks

### 01-foundation: Convert project and establish architecture

Replace the legacy project format with SDK-style .NET 8 Windows Forms, add required package references, create the documented project/folder boundaries, configure DI and logging, and add a test project with the initial domain/service test harness.

**Done when**: The solution restores and builds on .NET 8 with zero warnings, the application entry point resolves its DI graph, and the test project is discoverable.

---

### 02-workbook-storage: Implement validated ClosedXML persistence

Implement workbook initialization from the supplied template or exact fallback schema, validated reads, serialized async writes, atomic replacement with `.bak`, conflict/corruption handling, and backup/restore primitives.

**Done when**: Workbook creation, schema validation, CRUD round-trip, backup/restore, invalid input handling, and rapid serialized writes are covered by passing tests.

---

### 03-application-services: Implement task, category, settings, and reporting services

Create domain entities, DTOs, service interfaces, validation, task/category/settings operations, date calculations, metrics, and CSV export over the workbook store.

**Done when**: Services enforce all documented rules, preserve timestamps and category relationships, expose async APIs, and pass unit tests for acceptance cases.

---

### 04-ui-shell: Implement MainForm shell and visual system

Replace Form1 with the responsive MainForm, dark navy sidebar, light content area, navigation, common styling, error presentation, and page hosting infrastructure.

**Done when**: All eight navigation destinations open safely, keyboard focus is usable, layout resizes at the documented sizes, and the application launches without legacy generated UI.

---

### 05-task-pages: Implement Dashboard, All Tasks, Categories, and Task Editor

Implement task grid/search/filter/sort/quick-add/actions, category CRUD and deletion behavior, and the modal editor with validation and completion semantics.

**Done when**: Create/edit/complete/reopen/delete and category operations persist after restart, filters compose predictably, and invalid titles cannot be saved.

---

### 06-planning-pages: Implement Calendar View and Today’s Plan

Implement month navigation, selected-day filtering, due-today/overdue calculations, summary counts, completion toggles, and editor integration.

**Done when**: Calendar and today views match workbook data, date boundaries behave correctly, and edits/completions refresh immediately.

---

### 07-reports-settings: Implement Reports and Settings

Implement report metrics and CSV export plus preferences, backup/restore UI, validation, and friendly workbook error recovery.

**Done when**: Report totals reconcile with workbook data, empty periods show zero, CSV opens as valid tabular data, settings persist, and backup/restore round-trips safely.

---

### 08-qa-release: Run full validation and release checks

Build the complete solution, run all available tests, fix warnings, review empty/error/keyboard/DPI paths, and produce a self-contained Windows x64 publish output where the environment permits.

**Done when**: Full build is warning-free, tests pass, and remaining platform-only limitations are documented precisely.
