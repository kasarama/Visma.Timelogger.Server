using Visma.Timelogger.Api.Middleware;
using Visma.Timelogger.Application;
using Visma.Timelogger.Persistence;

namespace Visma.Timelogger.Api
{
    public static class StartupExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();


                using var scope = app.Services.CreateScope();
                try
                {
                    var context = scope.ServiceProvider.GetService<ProjectDbContext>();
                    if (context != null)
                    {
                      context.Database.EnsureCreatedAsync();
                      
                    }
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            app.UseHttpsRedirection();

            app.UseCustomHandlers();

            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => "Visma e-conomic Time Logger");

            return app;
        }
    }
}
