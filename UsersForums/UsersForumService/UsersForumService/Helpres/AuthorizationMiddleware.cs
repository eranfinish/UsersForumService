
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
namespace UsersForumService.Helpres
{
    public class AuthorizationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;


        public AuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                if (ValidateToken(token))
                {
                    await _next(context); // Continue down the pipeline if the token is valid
                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    await context.Response.WriteAsync("Invalid Token");
                }
            }
            else
            {
                // No token found
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Token is required");
            }
        }

        private bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  // Remove default clock skew
                }, out SecurityToken validatedToken);

                // Additional custom validation logic can go here

                return true; // Return true if token is valid
            }
            catch
            {
                return false; // Return false if validation fails
            }

        }
    }
}
