namespace Visma.Timelogger.Api.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomHandlers(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>()
                          .UseMiddleware<AuthorizationMiddleware>()
                          ;
        }
    }
}
