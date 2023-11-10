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

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration configuration;

        public AuthenticationController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        [HttpPost("registrate")]
        public IActionResult Registrate([FromBody] RegistrateModel registrateModel, IUnitOfWork unit)
        {
            if (registrateModel is null)
            {
                return BadRequest("Invalid client request");
            }

            unit.UserRepo.Add(new User
            {
                Name = registrateModel.Login,
                Password = registrateModel.Password
            });

            unit.Save();

            return Ok();
        }

        [HttpPost("getjwt")]
        public IActionResult GetJwt([FromBody] GetJWTModel getJWTModel, IUnitOfWork unit)
        {
            if (getJWTModel is null)
            {
                return BadRequest("Invalid client request");
            }

            var user = unit.UserRepo.ReadFirst((user) => getJWTModel.Login == user.Name);

            if (user is null)
            {
                return Unauthorized();
            }

            if (user?.Password == getJWTModel?.Password)
            {
                string? JWTKey = configuration.GetSection("Keys").GetSection("JWTKey").Value;
                if (JWTKey is null) { return StatusCode(500); }

                var claimList = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7021",
                    audience: "https://localhost:7021",
                    claims: claimList,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new AuthenticatedResponse { Token = tokenString });
            }
            return Unauthorized();
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
            User? user = unit.UserRepo.ReadFirst(user => user.Name == claimsIdentity.Name);
            return user;
        }
    }
}
