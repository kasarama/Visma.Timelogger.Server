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
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => "Visma e-conomic Time Logger");

            return app;
        }
    }
}
