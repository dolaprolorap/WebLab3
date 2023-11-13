using backend.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using backend.Models.DB;
using backend.Models.API.Auth;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;
        private ITokenService tokenService;

        public AuthenticationController(IConfiguration iConfig, ITokenService tokenService)
        {
            this.configuration = iConfig;
            this.tokenService = tokenService;
        }

        [HttpPost("registrate")]
        public IActionResult Registrate([FromBody] RegistrateModel registrateModel, IUnitOfWork _unit)
        {
            if (registrateModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var user = _unit.UserRepo.ReadFirst(user => user.UserName == registrateModel.Login);
            if (user is not null) return Ok("Username is already in used");

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, registrateModel.Login)
            };

            var token = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();

            _unit.UserRepo.Add(new User
            (
                guid : Guid.NewGuid(),
                name : registrateModel.Login,
                password: registrateModel.Password,
                refreshToken: refreshToken,
                refreshTokenExp: DateTime.Now.AddDays(14)
            ));

            _unit.Save();

            return Ok(new AuthenticatedResponse
            {
                Token = token,
                RefreshToken = refreshToken,
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel, IUnitOfWork _unit)
        {
            if (loginModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var user = _unit.UserRepo.ReadFirst(user => (user.UserName == loginModel.Login) && (user.Password == loginModel.Password));
            if (user is null) return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Login),
            };

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(14);

            _unit.Save();

            return Ok(new AuthenticatedResponse
            {
                Token = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("getuser")]
        [Authorize]
        public IActionResult GetUser(IUnitOfWork unit)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity is null) return Unauthorized("There is no identity");
            var user = GetUserByIdentity(claimsIdentity, unit);
            return Ok(user);
        }

        [NonAction]
        public User? GetUserByIdentity(ClaimsIdentity claimsIdentity, IUnitOfWork unit)
        {
            User? user = unit.UserRepo.ReadFirst(user => user.UserName == claimsIdentity.Name);
            return user;
        }
    }
}
