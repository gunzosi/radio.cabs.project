using AuthenticationService.DTOs;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly AuthenticationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public UserAuthController(AuthenticationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new
                    {
                        Status = 400,
                        Message = "User already exists"
                    });
                }
                var user = new User
                {
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    Password = PasswordHelper.HashPassword(userDto.Password)
                };
            
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var token = JwtHelper.GenerateToken(user.Email, _configuration["Jwt:Key"], "User");
                return Ok(new
                {
                    Status = 200,
                    Message = "User registered successfully",
                    Token = token
                });    
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = e.Message
                });
            } 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Identifier);
                if (existingUser == null || !PasswordHelper.VerifyPassword(loginDto.Password, existingUser.Password))
                {
                    return Unauthorized(new
                    {
                        Status = 401,
                        Message = "Invalid credentials - Password Incorrect"
                    });
                }
                
                var token = JwtHelper.GenerateToken(existingUser.Email, _configuration["Jwt:Key"], "User");
                
                existingUser.RefreshToken = Guid.NewGuid().ToString();
                existingUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
                await _dbContext.SaveChangesAsync();
                // Response.Cookies.Append("refreshToken", existingUser.RefreshToken, new CookieOptions
                // {
                //     HttpOnly = true,
                //     Secure = false,
                //     Expires = existingCompany.RefreshTokenExpiryTime
                // });
                return Ok(new
                {
                    StatusCode = 200,
                    Data = existingUser,
                    Token = token
                });
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "Error by Server , check try block at Login method of UserAuthController"
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefresh tokenRefresh)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == tokenRefresh.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    Message = "You are not authorized to access this resource"
                });
            }
            
            var tokenString = JwtHelper.GenerateToken(user.Email,_configuration["Jwt:Key"], user.Role);
            user.RefreshToken = Guid.NewGuid().ToString();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(20);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                StatusCode = 200,
                Token = tokenString,
                RefreshToken = user.RefreshToken
            });
        }
    }
}
