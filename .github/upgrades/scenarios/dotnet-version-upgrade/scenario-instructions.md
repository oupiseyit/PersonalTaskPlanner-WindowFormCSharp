# Personal Task Planner .NET 8 Implementation

## Strategy
Implement the documented roadmap phase by phase while converting the legacy WinForms project to SDK-style .NET 8 Windows Forms and restructuring it into Domain/Application/Infrastructure/WinForms/tests.

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net8.0-windows
- **Commit Strategy**: Manual (git is unavailable in this environment)
- **Data Storage**: ClosedXML single planner-data.xlsx workbook; no SQL database
- **Testing**: Build and run all available tests; fix warnings in modified projects

## Decisions
- Use the existing template workbook when available and create the documented schema when it is not.
- Preserve the documented UI conventions while replacing the generated legacy Form1 shell with the planned page architecture.

## Custom Instructions
<!-- Task-specific overrides: "For {taskId}: {instruction}" -->
