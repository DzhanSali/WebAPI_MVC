using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Model;
using WebAPI.Repository;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly DBConnection _context;

        public LoginController(IConfiguration config, DBConnection context)
        {
            _config = config;
            _context = context;
        }

        private Person Authenticate(LoginClass userLogin)
        {
            return _context.People.FirstOrDefault(o =>
                o.Name == userLogin.Name && o.Password == userLogin.Password);
            // if null redirect to action Login
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginClass userLogin)
        {
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim("Password", userLogin.Password),
                new Claim("Name", userLogin.Name)
            };

                var tokenOptions = new JwtSecurityToken(
                    _config["JWT:Issuer"],
                    _config["JWT:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);


                return Ok(tokenString);
            }
            return Unauthorized();
        }

        /*private string GenerateToken(Person user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Password", user.Password),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var tokenOptions = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    */
    }
}
