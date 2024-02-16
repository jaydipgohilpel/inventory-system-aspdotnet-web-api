using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace inventory_system_aspdotnet_web_api.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        IConfiguration _config;
        const string CustomUserIdClaimType = "userId";
        public JwtAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _config = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request is for a controller annotated with [AllowAnonymous]
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                // Skip authentication for controllers annotated with [AllowAnonymous]
                await _next(context);
                return;
            }

            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validate and decode token
            
            var claimsPrincipal = ValidateAndDecodeToken(token);

            // Access user ID from claims
            var userId = claimsPrincipal.FindFirst(CustomUserIdClaimType)?.Value;

            // Attach user ID to the current request context
            context.Items["userId"] = userId;

            await _next(context);
        }

        private ClaimsPrincipal ValidateAndDecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };

            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            return claimsPrincipal;
        }
    }
}
