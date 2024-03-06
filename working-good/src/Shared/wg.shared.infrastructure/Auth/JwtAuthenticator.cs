using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using wg.shared.abstractions.Auth;
using wg.shared.abstractions.Auth.DTOs;
using wg.shared.abstractions.Time;
using wg.shared.infrastructure.Auth.Configuration.Models;

namespace wg.shared.infrastructure.Auth;

internal sealed class JwtAuthenticator : IAuthenticator
{
    private readonly IClock _clock;
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public JwtAuthenticator(IClock clock, IOptions<JwtOptions> options)
    {
        _clock = clock;
        _options = options.Value;
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(options.Value.SigningKey)),SecurityAlgorithms.HmacSha256);
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    
    public JwtDto CreateToken(string userId, string role)
    {        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };
        var now = _clock.Now();
        var expiry = now.Add(_options.Expiry);
        var jwt = new JwtSecurityToken(_options.Issuer, _options.Audience, claims,
            now, expiry, _signingCredentials);
        var token = _jwtSecurityTokenHandler.WriteToken(jwt);
        return new JwtDto
        {
            Token = token
        };
    }
}