using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using TeknorixAPI.Domain.DTOs;
using TeknorixAPI.Global;

namespace TeknorixAPI.Helpers
{
    public class JwtTokenHelper
    {
        public TokenDto CreateToken(AuthorizedApp authApp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuedAt = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddDays(1);
            var claimsIdentity = new ClaimsIdentity(new GenericIdentity(authApp.AppName), new[]
            {
            new Claim("appId", authApp.AppId, ClaimValueTypes.String),
            });

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(AuthorizedKeys.Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //create the token
            var token = tokenHandler.CreateJwtSecurityToken(
                AuthorizedKeys.Issuer,
                AuthorizedKeys.Audience,
                claimsIdentity,
                issuedAt,
                expires,
                signingCredentials: signingCredentials);

            return new TokenDto
            {
                Token = tokenHandler.WriteToken(token),
                Expires = expires,
            };
        }
    }


}
