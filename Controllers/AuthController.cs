using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HHSurvey.Data;
using HHSurvey.Models;
using HHSurvey.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mono.TextTemplating;

namespace HHSurvey.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDbContext _context;

        public readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext context, IUserService userService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetAuth()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new response { message = "Failed", error = "Unauthorized Access" });
            }
           
            return await _context.UserLogin.ToListAsync();
        }

        [HttpPost("signup")]
        public async Task<ActionResult<response>> Signup([FromBody] RegisterDTO registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new response { message = "Failed", error = "Invalid paramenter" });
            }

            if (!_userService.FindUser(registerDto.username))
            {
                _userService.CreateUser(registerDto);
                return Created("", new response { message = "User created successfully", error = "" });
            }
            else
            {
                return Created("", new response { message = "User already exists", error = "" });

            }

         
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginDTO loginDto)
        {  string clientIp = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                  ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                  ?? "Unknown";
                   string deviceInfo = Request.Headers["User-Agent"].ToString();
            try
            {
              
                if (loginDto == null)
                {
                    return BadRequest("Invalid client request");
                }

                var user = _userService.Authenticate(loginDto.Username, loginDto.Password);

                if (user == null)
                { 
                    await SaveLoginHistory(loginDto.Username, "Failed", clientIp, deviceInfo);
                    return Unauthorized("Invalid username or password");
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
            {
            new Claim(ClaimTypes.GivenName, user.fullName),
            new Claim(ClaimTypes.Name, user.username),
            new Claim(ClaimTypes.Role, user.roleId.ToString()),
            new Claim(ClaimTypes.Email, user.emailId ?? "")
            };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: signinCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                await SaveLoginHistory(user.username, "Success", clientIp, deviceInfo);
                string? village = user.village; // from DB

                string[] villageArray = string.IsNullOrWhiteSpace(village)
                                    ? Array.Empty<string>()
                                    : village.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(v => v.Trim())
                                    .ToArray();
                return Ok(new { Token = token, Status="OK", Message="SUCCESS", State=user?.state, District=user?.district, Block=user?.block, GP=user?.panchayat, Village=villageArray });
            }   
            catch (Exception ex)
            {
               await  SaveLoginHistory(loginDto.Username, "Error : " + ex.Message.ToString(), clientIp, deviceInfo);
                _logger.LogError(ex, "Error on Login");
                return Ok(new { Token = "", Status="Error", Message="Something went wrong" });
            }
        }
        private async Task SaveLoginHistory(string? username, string status, string ip, string device)
        {
            var log = new LoginHistory
            {
                UserId = username ?? "",
                Status = status,
                IpAddress = ip,
                DeviceInfo = device,
                LoginTime = DateTime.Now
            };

            _context.LoginHistory.Add(log);
            await _context.SaveChangesAsync();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<response>> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            // 1. Basic Model Validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new response { message = "Failed", error = "Invalid data format or missing required fields." });
            }

            // 2. Retrieve User Identifier from JWT Claims
            // This is how you get the ID of the currently logged-in user
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(new response { message = "Failed", error = "Unauthorized Access" });
            }
            if (User.Identity.Name == null)
            {
                // This should generally not happen if [Authorize] is used correctly
                return Unauthorized(new response { message = "Failed", error = "User identity not found." });
            }

            // Assuming your User ID is a string (e.g., GUID or username)
            var userId = User.Identity.Name;

            // 3. Business Logic Execution
            try
            {
                // Call the service to perform the password change
                bool success = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

                if (success)
                {
                    return Ok(new response { message = "Password changed successfully.", error = "" });
                }
                else
                {
                    // This is typically for "Current password is incorrect"
                    return BadRequest(new response { message = "Failed", error = "Invalid current password." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (recommended) and return a generic error
                // _logger.LogError(ex, "Error changing password for user {UserId}", userId);
                return StatusCode(500, new response { message = "Failed", error = "An internal server error occurred." });
            }
        }


        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // Validate the refresh token
            if (request.RefreshToken == null)
            {
                return BadRequest("Invalid refresh token");
            }

            // Here you would typically validate the refresh token against your database
            // For simplicity, we assume the refresh token is valid

            // Generate a new access token
            var newAccessToken = GenerateAccessToken(request.Username);

            // Optionally, you can generate a new refresh token
            var newRefreshToken = GenerateRefreshToken();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        private string GenerateAccessToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15), // Short expiration time for access token
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
public class RefreshTokenRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}