using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Humanizer.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenservice
{
    public string CreateToken(AppUser user)
    {
        var tokenkey= config["Tokenkey"]?? throw new Exception ("Cannot access the token form appsettings");
        if(tokenkey.Length <64) throw new Exception (" Token key need to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenkey));

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,user.UserName)
        };
        var creds =new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor =new SecurityTokenDescriptor
        {
            Subject= new ClaimsIdentity(claims),
            Expires =DateTime.UtcNow.AddDays(7),
            SigningCredentials =creds,

        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
