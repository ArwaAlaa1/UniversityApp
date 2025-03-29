using Lab_1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lab_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid Password");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = CreateToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);
            if (userExists != null)
                return BadRequest("User already exists!");

            var user = new IdentityUser
            {
                
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

       
            await _userManager.AddToRoleAsync(user, "Student");

            return Ok("User created successfully!");
        }

        [HttpPost("add-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto)
        {
            if (await _roleManager.RoleExistsAsync(roleDto.Name))
                return BadRequest(new {message= "Role already exists" });

            await _roleManager.CreateAsync(new IdentityRole(roleDto.Name));
            return Ok(new { message = "Role created successfully" });
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:secretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:validIssuer"],
                audience: _configuration["Jwt:validAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
