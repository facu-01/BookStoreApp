using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {

            var user = new ApiUser
            {
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded == false)
            {
                foreach (var err in result.Errors)
                {

                    ModelState.AddModelError(err.Code, err.Description);
                    return BadRequest(ModelState);

                }
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return NoContent();

        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserAuthenticatedDto>> Login(UserLoginDto loginUserDto)
        {

            var user = await _userManager.FindByNameAsync(loginUserDto.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            var passWordValid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

            if (passWordValid == false)
            {
                return Unauthorized();
            }

            string tokenString = await GenerateToken(user);

            return new UserAuthenticatedDto
            {
                UserName = loginUserDto.UserName,
                Token = tokenString,
                UserId = user.Id
            };

        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Sub,user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("uid", user.Id),
                new(CustomClaimTypes.Uid, user.Id),
            }
            .Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:Duration"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
