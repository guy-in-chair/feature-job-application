using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using TeknorixAPI.Global;

namespace TeknorixAPI.Filters
{
    public class TokenValidationFilter : IAsyncAuthorizationFilter
    {
        private static bool TryRetrieveToken(HttpRequest request, out string token)
        {
            token = null;
            var authzHeaders = request.Headers["Authorization"].ToList();
            if (authzHeaders.Count != 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.Single();
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var request = context.HttpContext.Request;

            HttpStatusCode statusCode;
            string token;

            // List of allowed unauthorized paths
            // for sms endpoints there will be twilio authentication
            var allowedUnauthorizedPaths = new List<string>
            {
                "/authorize/token"
            };

            // Get the request path
            string requestPath = request.Path.Value;

            //Check if the unauthorized request is allowed:
            bool isUnauthorizedRequestAllowed = allowedUnauthorizedPaths.Any(allowedUnauthorizedPath => requestPath.ToLower().Contains(allowedUnauthorizedPath.ToLower()));

            // Determine whether a JWT exists or not
            if (!TryRetrieveToken(request, out token))
            {
                if (!isUnauthorizedRequestAllowed)
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    context.Result = new StatusCodeResult((int)statusCode);
                    return;
                }
            }

            if (!isUnauthorizedRequestAllowed)
            {
                try
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(AuthorizedKeys.Secret));

                    SecurityToken securityToken;
                    var handler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = AuthorizedKeys.Audience,
                        ValidIssuer = AuthorizedKeys.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        LifetimeValidator = LifetimeValidator,
                        IssuerSigningKey = securityKey
                    };

                    // Extract and assign the user of the JWT
                    var principal = handler.ValidateToken(token, validationParameters, out securityToken);
                    context.HttpContext.User = principal;

                    return;
                }
                catch (SecurityTokenValidationException e)
                {
                    statusCode = HttpStatusCode.Unauthorized;
                }
                catch (Exception ex)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }

                context.Result = new StatusCodeResult((int)statusCode);
            }
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null && DateTime.UtcNow < expires)
            {
                return true;
            }
            return false;
        }
    }

}
