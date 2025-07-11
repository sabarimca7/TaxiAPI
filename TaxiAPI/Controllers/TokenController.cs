using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaxiAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("generate")]
        public IActionResult GenerateToken()
        {
            // ✅ Check Origin or Referer header
            var origin = Request.Headers["Origin"].ToString();
                    var allowedOrigins = new[] {
                   "https://www.gooddaytaxi.com/",
                   "http://www.gooddaytaxi.com/",
                   "https://localhost:4200/",
                   "http://localhost:4200/"
            };

            if (!allowedOrigins.Contains(origin))
            {
                return Unauthorized("Invalid origin.");
            }         

            // ✅ Generate token...
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = tokenString });
        }
    }
}
