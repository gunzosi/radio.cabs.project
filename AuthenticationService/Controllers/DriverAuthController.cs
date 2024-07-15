using AuthenticationService.DTOs;
using AuthenticationService.Models;
using AuthenticationService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverAuthController : ControllerBase
    {
        private readonly AuthenticationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public DriverAuthController(AuthenticationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var existingDriver = await _dbContext.Drivers.FirstOrDefaultAsync(d =>
                    d.DriverMobile == loginDto.Identifier);
                if (existingDriver == null || !PasswordHelper.VerifyPassword(loginDto.Password, existingDriver.Password))
                {
                    return Unauthorized();
                }

                var token = JwtHelper.GenerateToken(existingDriver.DriverMobile.ToString(), _configuration["Jwt:Key"], "Driver");
                existingDriver.RefreshToken = Guid.NewGuid().ToString();
                existingDriver.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
                await _dbContext.SaveChangesAsync();
                
                return Ok(new
                {
                    Message = "Login successful",
                    Driver = existingDriver,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "There is an error for Request"
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(DriverDto driverDto)
        {
            try
            {
                // if (driverDto.DriverMobile.ToString().StartsWith("0"))
                // {
                //     return BadRequest(new { message = "DriverMobile should not start with zero." });
                // }

                var existingDriver =
                    await _dbContext.Drivers.FirstOrDefaultAsync(d => d.DriverMobile == driverDto.DriverMobile || d.DriverEmail == driverDto.DriverEmail);
                if (existingDriver != null)
                {
                    return Conflict(new
                    {
                        message = "Driver with this mobile number or email already exists"
                    });
                }

                driverDto.DriverCode = Guid.NewGuid().ToString();

                var driver = new Driver
                {
                    DriverName = driverDto.DriverName,
                    DriverCode = driverDto.DriverCode,
                    DriverMobile = driverDto.DriverMobile,
                    DriverEmail = driverDto.DriverEmail,
                    Password = PasswordHelper.HashPassword(driverDto.Password),
                    DriverLicense = driverDto.DriverLicense,
                    Address = driverDto.Address,
                    Street = driverDto.Street,
                    Ward = driverDto.Ward,
                    District = driverDto.District,
                    City = driverDto.City,
                    CompanyId = null // Set CompanyId to null if not provided
                };

                await _dbContext.Drivers.AddAsync(driver);
                await _dbContext.SaveChangesAsync();

                var token = JwtHelper.GenerateToken(driver.DriverMobile.ToString(), _configuration["Jwt:Key"], "Driver");
                return Ok(new
                {
                    Message = "Driver registered successfully",
                    Driver = driver,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    message = e.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefresh tokenRefreshDto)
        {
            var driver =
                await _dbContext.Drivers.FirstOrDefaultAsync(d => d.RefreshToken == tokenRefreshDto.RefreshToken);
            if (driver == null || driver.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var tokenString = JwtHelper.GenerateToken(driver.DriverMobile.ToString(), _configuration["Jwt:Key"], "Driver");
            driver.RefreshToken = Guid.NewGuid().ToString();
            driver.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Token = tokenString,
                RefreshToken = driver.RefreshToken
            });
        }
    }
}
