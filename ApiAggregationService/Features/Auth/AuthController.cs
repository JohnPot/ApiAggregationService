using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAggregationService.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpPost("token")]
    public IActionResult CreateToken()
    {
        var jwt = _configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "test-user")
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );


        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );


        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );


        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);


        return Ok(new
        {
            token = tokenString
        });
    }
}