using Api.DTO;
using Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(UserManager<ApplicationUser> user, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = user;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO RDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = RDTO.UserName,
                Email = RDTO.Email,
                PhoneNumber = RDTO.Phone
            };
            var createUserResult = await _userManager.CreateAsync(applicationUser, RDTO.Password);
            if (!createUserResult.Succeeded)
            {
                return BadRequest(new { errors = createUserResult.Errors.Select(e => e.Description) });
            }

            if (RDTO.NameRole == "Admin" || RDTO.NameRole == "admin")
            {
                var roleExist = await _roleManager.RoleExistsAsync("Admin");
                if (!roleExist)
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _userManager.AddToRoleAsync(applicationUser, "Admin");
            }
            else
            {
                var roleExist = await _roleManager.RoleExistsAsync("User");
                if (!roleExist)
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                await _userManager.AddToRoleAsync(applicationUser, "User");
            }

            return Ok(new { message = "Operation completed successfully" });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserModel = await _userManager.FindByEmailAsync(login.Email);
            if (UserModel == null)
            {
                return BadRequest(new { error = "Email not found" });
            }
            if (!await _userManager.CheckPasswordAsync(UserModel, login.Password))
            {
                return BadRequest(new { error = "Incorrect password" });
            }
            var roles = await _userManager.GetRolesAsync(UserModel);
            var claims = new List<Claim>
            {
                new Claim("Email", UserModel.Email),
                new Claim("Id", UserModel.Id),
                new Claim("UserName", UserModel.UserName),
                new Claim("PhoneNumber", UserModel.PhoneNumber ?? ""),
                new Claim("role", roles[0]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> claims)
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                return new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
                );
            }
            var token = GenerateJwtToken(claims);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
