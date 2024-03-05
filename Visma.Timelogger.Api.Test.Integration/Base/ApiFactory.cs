using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Visma.Timelogger.Persistence;


namespace Visma.Timelogger.Api.Test.Integration.Base
{
    public class ApiFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _dbName;

        public ApiFactory(string dbName)
        {
            _dbName = dbName;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ProjectDbContext>));

                services.Remove(dbContextDescriptor);


                services.AddDbContext<ProjectDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });
            });
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var _context = scopedServices.GetRequiredService<ProjectDbContext>();
                    _context.Database.EnsureCreated();
                    _context.Projects.Add(TestData.ActiveProject);
                    _context.Projects.Add(TestData.InactiveProject);
                    _context.SaveChanges();
                }
            });
        }
    
        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }
    }
}


