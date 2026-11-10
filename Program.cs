using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalTaskPlanner.Application;
using PersonalTaskPlanner.Infrastructure;
using PersonalTaskPlanner.WinForms;

namespace PersonalTaskPlanner;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var services = ConfigureServices();
        global::System.Windows.Forms.Application.Run(services.GetRequiredService<MainForm>());
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Information));
        services.AddSingleton<IPlannerStore, ExcelWorkbookStore>();
        services.AddSingleton<ITaskService, TaskService>();
        services.AddSingleton<ICategoryService, CategoryService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddTransient<MainForm>();
        return services.BuildServiceProvider();
    }
}
