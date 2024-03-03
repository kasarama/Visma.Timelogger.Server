using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Services;

namespace Visma.Timelogger.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IRequestValidator, RequestValidator>();

            return services;
        }
    }
}
