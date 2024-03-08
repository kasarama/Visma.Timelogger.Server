using Visma.Timelogger.Application.Features;

namespace Visma.Timelogger.Api.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("User", out var accessToken))
            {
                _logger.LogWarning("Header 'Authorization' is missing from IP: {ip}.", context.Connection.RemoteIpAddress);

                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }
            else
            {
                // Validate the access token with the Authentication Service
                UserInfo userInfo = ValidateAccessToken(accessToken);
                if (userInfo.UserId == Guid.Empty)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authorization header is invalid.");
                    return;
                }
                else
                {
                    context.Items["UserId"] = userInfo.UserId;
                }
            }
            // Call the next middleware in the pipeline
            await _next(context);
        }

        public UserInfo ValidateAccessToken(string accessToken)
        {
            UserInfo userInfo = new UserInfo();
            switch (accessToken.ToLower())
            {
                case "freelancer1":
                    userInfo.UserId = Guid.Parse("486E3E8F-0DC3-4E00-8711-BE3A6CB1399E");
                    break;
                case "freelancer2":
                    userInfo.UserId = Guid.Parse("DD330056-EE5A-451B-AC2C-AF0CB20EB213");
                    break;
                case "customer1":
                    userInfo.UserId = Guid.Parse("671758F5-320D-4D1C-8C1A-54CDC55F2F75");
                    break;
                case "customer2":
                    userInfo.UserId = Guid.Parse("AE1E5C8E-7106-401A-A51F-FBEC70972DEE");
                    break;
                default:
                    userInfo.UserId = Guid.Empty;
                    break;
            }

            return userInfo;
        }
    }
}
