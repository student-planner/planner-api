using Microsoft.Extensions.Options;
using Planner.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using Seljmov.AspNet.Commons.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Planner.API.Helpers;

public class JwtCreator
{
    private const int RefreshTokenLength = 64;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly ILogger<JwtCreator> _logger;

    /// <summary>
    /// Конструктор класса <see cref="JwtCreator"/>
    /// </summary>
    /// <param name="jwtOptions">Настройки jwt</param>
    public JwtCreator(IOptions<JwtOptions> jwtOptions, ILogger<JwtCreator> logger)
    {
        _jwtOptions = jwtOptions;
        _logger = logger;
    }

    /// <summary>
    /// Прочитать Jwt
    /// </summary>
    /// <param name="token">Jwt</param>
    /// <param name="claims">Данные пользователя</param>
    /// <param name="validTo">Дата валидности</param>
    /// <returns></returns>
    public bool ReadAccessToken(string token, out ClaimsPrincipal? claims, out DateTime validTo)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validations = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtOptions.Value.GetSymmetricSecurityKey(),
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Value.Audience,
            ValidateLifetime = true
        };

        try
        {
            claims = tokenHandler.ValidateToken(token, validations, out var validatedToken);
            validTo = validatedToken.ValidTo;
            return true;
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(message: ex.ToString());
        }

        claims = null;
        validTo = DateTime.MinValue;
        return false;
    }

    /// <summary>
    /// Создать токен доступа
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="minutesValid">Время действия токена</param>
    /// <returns>Токен доступа</returns>
    public string CreateAccessToken(User user, int minutesValid)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimsIdentity.DefaultIssuer, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Value.Issuer,
            Audience = _jwtOptions.Value.Audience,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(minutesValid),
            Subject = subject,
            SigningCredentials = new SigningCredentials(_jwtOptions.Value.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Создать токен обновления
    /// </summary>
    /// <returns>Токен обновления</returns>
    public static string CreateRefreshToken()
    {
        var token = RandomNumberGenerator.GetBytes(RefreshTokenLength);
        var bytes = Encoding.UTF8.GetBytes(Convert.ToBase64String(token));
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
