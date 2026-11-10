# Excel Workbook Storage Specification (.xlsx)

## Goal
Persist all app data in a single local Excel workbook (`planner-data.xlsx`) instead of SQLite or another database. Use **ClosedXML** in C# .NET 8 WinForms. Excel is the storage file, not a live Excel automation dependency; Microsoft Excel need not be installed.

## Location and startup
- Default: `%LOCALAPPDATA%\PersonalTaskPlanner\planner-data.xlsx` (writable per-user folder, not Program Files).
- On first run, copy `templates/planner-data.xlsx` to the default path; if absent, create a workbook with the exact headers below.
- If the file is missing, create it. If corrupt, locked, or wrong schema, show a clear error and offer a backup restore; never silently overwrite existing data.
- Only one application instance should write the workbook at a time. Excel may open it for reading, but if it locks the file, save must report the conflict and retry only with user approval.

## Sheets and columns (row 1 is the header)

### Tasks
| Column | Type | Rules |
|---|---|---|
| Id | GUID string | Unique, immutable |
| Title | Text | Required, max 200 |
| Description | Text | Optional |
| DueAt | ISO 8601 datetime string with offset | Optional |
| Priority | Text | Low / Medium / High / Critical |
| Status | Text | Open / Completed |
| CategoryId | GUID string | Optional, matches Categories.Id |
| CreatedAt | UTC ISO 8601 string | Required |
| UpdatedAt | UTC ISO 8601 string | Required |
| CompletedAt | UTC ISO 8601 string | Blank until completed |

### Categories
| Column | Type | Rules |
|---|---|---|
| Id | GUID string | Unique |
| Name | Text | Required, unique case-insensitively |
| ColorHex | Text | Optional `#RRGGBB` |
| CreatedAt | UTC ISO 8601 string | Required |

### Settings
| Column | Type | Rules |
|---|---|---|
| Key | Text | Unique |
| Value | Text | String or serialized JSON |

## Relationship and data rules
- Tasks.CategoryId references Categories.Id in application logic; Excel has no foreign-key enforcement.
- When deleting a category, clear its ID from affected tasks.
- Validate all fields before saving. Reject duplicate IDs and duplicate category names.
- Use stable GUIDs for tasks/categories, not Excel row numbers.
- Reports and calendar read from Tasks sheet via application services, never directly from UI controls.
- All date comparisons use a documented local timezone; CreatedAt, UpdatedAt and CompletedAt are UTC.

## Safe read/write design
1. Load workbook through `IExcelWorkbookStore` into domain objects.
2. Validate requested mutation in `TaskService` / `CategoryService`.
3. Acquire an in-process `SemaphoreSlim` lock and ensure a single app writer.
4. Re-read current workbook before mutation if external changes may have occurred; detect conflicts.
5. Save the complete workbook to a temporary `.xlsx` in the same directory.
6. Replace the original atomically where supported, retaining a `.bak` copy; do not write directly to the original.
7. Release the lock and refresh bound controls; show actionable errors on failures.

## Backups, import/export and limitations
- Provide manual Backup and Restore in Settings, plus timestamped backups before destructive restore.
- Validate required sheets/headers on restore and use a safe replace.
- Export reports to CSV independently of workbook storage.
- Suitable for a single-user desktop planner with modest data volumes. Not suitable for concurrent multi-user editing or large datasets; migrate to a database if those needs arise.
- Excel formulas are not required in the data sheets; application code computes metrics.

## Acceptance tests
- First launch creates valid workbook; subsequent launches preserve data.
- Task/category CRUD persists across restart and all pages show consistent data.
- Completing/reopening a task updates timestamps correctly.
- Category deletion leaves related tasks Uncategorized.
- Locked workbook and corrupt workbook fail safely without data loss.
- Backup and restore round-trip all sheets; rapid successive writes do not lose updates.
