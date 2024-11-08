using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeknorixAPI.Domain.DTOs;
using TeknorixAPI.Global;
using TeknorixAPI.Helpers;

namespace TeknorixAPI.Controllers
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private readonly AuthorizedApp _authApp = null; 
        private readonly JwtTokenHelper _tokenHelper = null;

        public AuthorizeController()
        {
            _authApp = new AuthorizedApp();
            _tokenHelper = new JwtTokenHelper();
        }

        [Route("token")]
        [HttpPost]
        public object GetToken(AuthorizeRequestDto authorizeRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authApp.AppId.Equals(authorizeRequest.AppId) || !_authApp.AppSecret.Equals(authorizeRequest.AppSecret))
                return Unauthorized();

            var token = _tokenHelper.CreateToken(_authApp);
            return Ok(token);

        }
    }


}
