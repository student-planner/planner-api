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

    /// <summary>
    /// Конструктор класса <see cref="JwtCreator"/>
    /// </summary>
    /// <param name="jwtOptions">Настройки jwt</param>
    public JwtCreator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
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
