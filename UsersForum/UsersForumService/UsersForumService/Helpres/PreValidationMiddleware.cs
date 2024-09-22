using System.IdentityModel.Tokens.Jwt;

namespace UsersForumService.Helpres
{
    public class PreValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PreValidationMiddleware> _logger;
        public PreValidationMiddleware(RequestDelegate next, ILogger<PreValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                // Parse JWT and extract claims
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var claims = jsonToken.Claims;
                    // Add claims or specific data to HttpContext.Items for later access
                    context.Items["UserClaims"] = claims;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
