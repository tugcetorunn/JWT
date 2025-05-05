using JWT.Models;
using JWT.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public LoginController(UserManager<AppUser> _userManager, IConfiguration _configuration)
        {
            userManager = _userManager;
            configuration = _configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı");

            var isCorrectPassword = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!isCorrectPassword)
                return Unauthorized("Şifre hatalı");

            // Token oluşturma işlemi
            return Ok(GetToken(dto.UserName));
        }

        private string GetToken(string email)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:secretKey"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                //issuer: configuration["JwtSettings:validIssuer"],
                //audience: configuration["JwtSettings:validAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signin
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
