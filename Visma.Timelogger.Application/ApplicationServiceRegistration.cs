using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Visma.Timelogger.Application.Contracts;
using Visma.Timelogger.Application.Features.CreateTimeRecord;
using Visma.Timelogger.Application.Features.GetProjectOverview;
using Visma.Timelogger.Application.Services;

namespace Visma.Timelogger.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IApiRequestValidator, HandlerService>();
            services.AddScoped<AbstractValidator<CreateTimeRecordCommand>, CreateTimeRecordCommandValidator>();
            services.AddScoped<AbstractValidator<GetProjectOverviewQuery>, GetProjectOverviewQueryValidator>();

            return services;
        }
    }
}
