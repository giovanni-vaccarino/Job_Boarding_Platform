using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using backend.Shared.Enums;

namespace backend.Shared.Security;


public class SecurityContext
{
    private readonly JwtConfig _jwtConfig;

    public SecurityContext(IConfiguration configuration)
    {
        _jwtConfig = configuration.GetSection("Jwt").Get<JwtConfig>()
                     ?? throw new InvalidOperationException("JwtConfig missing in appsettings.json");
    }

    public string CreateAccessToken(User user)
    {
        return this.CreateJwtToken(user, TokenType.Access);
    }
    
    public string CreateRefreshToken(User user)
    {
        return this.CreateJwtToken(user, TokenType.Refresh);
    }
    
    public ClaimsPrincipal ValidateJwtToken(string token, TokenType expectedType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
        
        var tokenType = principal.FindFirst("token_type")?.Value;
        if (tokenType != expectedType.ToString())
        {
            throw new SecurityTokenException($"Token type mismatch. Expected: {expectedType}, Found: {tokenType}");
        }

        // If the token is valid I return the claims
        return principal;
    }

    
    public string Hash(string plain)
    {
        return BCrypt.Net.BCrypt.HashPassword(plain);
    }
    
    public bool ValidateHashed(string plain, string hashed)
    {
        return BCrypt.Net.BCrypt.Verify(plain, hashed);
    }

    private string CreateJwtToken(User user, TokenType tokenType)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);

        var expiryDate = tokenType == TokenType.Access
            ? DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryAccessToken)
            : DateTime.UtcNow.AddDays(_jwtConfig.ExpiryRefreshToken);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("tokenType", tokenType.ToString())
            }),
            Expires = expiryDate,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}