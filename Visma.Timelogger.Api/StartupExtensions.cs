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
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "dev policy",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .AllowCredentials();
                                  });
            });
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
            }
            app.UseCors("dev policy");

            app.UseHttpsRedirection();

            app.UseCustomHandlers();

            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => "Visma e-conomic Time Logger");

            return app;
        }
    }
}
