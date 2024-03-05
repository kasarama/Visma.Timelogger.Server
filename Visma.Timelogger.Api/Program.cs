using Visma.Timelogger.Api;
using Visma.Timelogger.Persistence;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

using var scope = app.Services.CreateScope();
try
{
    var context = scope.ServiceProvider.GetService<ProjectDbContext>();
    if (context != null)
    {
        context.Database.EnsureCreated();
    }
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
}
app.Run();

public partial class Program { }