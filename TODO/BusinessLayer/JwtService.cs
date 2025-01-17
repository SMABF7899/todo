﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TODO;

public class JwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(string secret, string issuer, string audience)
    {
        _secret = secret;
        _issuer = issuer;
        _audience = audience;
    }

    public string GenerateSecurityToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = signingCredentials
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool VerifySecurityToken(string jwt, string key, string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = true
        };

        try
        {
            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(jwt, validationParameters, out validatedToken);
            var nameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
            if (nameClaim == null || nameClaim.Value != username)
                return false;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}