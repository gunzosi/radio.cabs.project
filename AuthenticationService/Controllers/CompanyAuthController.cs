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
    public class CompanyAuthController : ControllerBase
    {
        private readonly AuthenticationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public CompanyAuthController(AuthenticationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CompanyDto companyDto)
        {
            try
            {
                var existingCompany =
                    await _dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyTaxCode == companyDto.CompanyTaxCode);
                if (existingCompany != null)
                {
                    return Conflict(new
                    {
                        message = "Company with this tax code or email already exists"
                    });
                }

                var company = new Company
                {
                    CompanyName = companyDto.CompanyName,
                    CompanyTaxCode = companyDto.CompanyTaxCode,
                    ContactPerson = companyDto.ContactPerson,
                    Designation = companyDto.Designation,
                    ContactPersonNumber = companyDto.CpMobile,
                    CompanyTelephone = companyDto.CompanyTelephone,
                    FaxNumber = companyDto.FaxNumber,
                    CompanyEmail = companyDto.CompanyEmail,
                    Password = PasswordHelper.HashPassword(companyDto.Password),
                    MembershipTypeId = null
                };
                
                // existingCompany.RefreshToken = Guid.NewGuid().ToString();
                // existingCompany.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
                
                await _dbContext.Companies.AddAsync(company);
                await _dbContext.SaveChangesAsync();
                var token = JwtHelper.GenerateToken(companyDto.CompanyTaxCode, _configuration["Jwt:Key"], "Company");
                return Ok(new
                {
                    Message = "Company registered successfully",
                    Company = company,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return BadRequest("Error by request to register company");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var existingCompany = await _dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyTaxCode == loginDto.Identifier);
                if (existingCompany == null ||
                    !PasswordHelper.VerifyPassword(loginDto.Password, existingCompany.Password))
                {
                    return Unauthorized("The TAX CODE / Password is incorrect");
                }
                
                var token = JwtHelper.GenerateToken(existingCompany.CompanyTaxCode, _configuration["Jwt:Key"], "Company");
                existingCompany.RefreshToken = Guid.NewGuid().ToString();
                existingCompany.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
                await _dbContext.SaveChangesAsync();
                // Response.Cookies.Append("refreshToken", existingCompany.RefreshToken, new CookieOptions
                // {
                //     HttpOnly = true,
                //     Secure = false,
                //     Expires = existingCompany.RefreshTokenExpiryTime
                // });
                return Ok(new
                {
                    Message = "Company logged in successfully",
                    Company = existingCompany,
                    Token = token
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    Message = "LOGIN Company - There was an error with the request from the client with the message: " + e.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRefresh refreshTokenDto)
        {
            var company =
                await _dbContext.Companies.FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken);
            if (company == null || company.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    Message = "Invalid refresh token for Company"
                });
            } 
            var tokenString = JwtHelper.GenerateToken(company.CompanyTaxCode, _configuration["Jwt:Key"], "Company");
            company.RefreshToken = Guid.NewGuid().ToString();
            company.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(60);
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Token refreshed successfully",
                Token = tokenString,
                RefreshToken = company.RefreshToken
            });
        }
        
    }
}
