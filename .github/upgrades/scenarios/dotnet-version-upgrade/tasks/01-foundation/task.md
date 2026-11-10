# 01-foundation: Convert project and establish architecture

## Objective
Replace the legacy project format with SDK-style .NET 8 Windows Forms, add required package references, create the documented project/folder boundaries, configure DI and logging, and add a test project with the initial domain/service test harness.

## Research findings
- The solution contains one legacy project: `PersonalTaskPlanner.csproj`, targeting `net472` with explicit System references and generated Form1/Properties files.
- `Program.cs` launches `Form1`; `Form1` only calls `InitializeComponent`, so the UI can be replaced without preserving business behavior.
- `get_project_dependencies` found no NuGet packages, no project references, and no imported Directory.Build/CPM files.
- Topological project order contains only `PersonalTaskPlanner.csproj`.
- The documented architecture requires Domain, Application, Infrastructure, WinForms, and tests boundaries; the implementation will keep the solution entry project as the WinForms host and add the layers as folders/projects where practical.
- Required packages are ClosedXML, Microsoft.Extensions.DependencyInjection, Microsoft.Extensions.Logging, and a test framework for the new test project.

## Scope
- Convert `PersonalTaskPlanner.csproj` to SDK-style `net8.0-windows` with WinForms enabled.
- Remove legacy generated compile/resource metadata that conflicts with SDK defaults.
- Add package references and test project structure.
- Create baseline domain/application/infrastructure/winforms folders and DI bootstrap.

**Done when**: The solution restores and builds on .NET 8 with zero warnings, the application entry point resolves its DI graph, and the test project is discoverable.
