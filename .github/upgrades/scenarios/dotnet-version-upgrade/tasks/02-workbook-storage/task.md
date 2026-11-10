# 02-workbook-storage: Implement validated ClosedXML persistence

Implement workbook initialization from the supplied template or exact fallback schema, validated reads, serialized async writes, atomic replacement with `.bak`, conflict/corruption handling, and backup/restore primitives.

**Done when**: Workbook creation, schema validation, CRUD round-trip, backup/restore, invalid input handling, and rapid serialized writes are covered by passing tests.
