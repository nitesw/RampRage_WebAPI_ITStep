using Core.Interfaces;
using Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class JwtService(
        IConfiguration configuration,
        UserManager<UserEntity> userManager
        ) : IJwtService
    {
        public async Task<string> CreateTokenAsync(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email!),
                new Claim("username", user.UserName!)
            };
            var role = await userManager.GetRolesAsync(user);

            claims.Add(new Claim("role", role[0]));

            var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSecretKey") ??
                "adlfjalUIQuihafqweqwe227rt74k765gy32lNLJLhfaASfy93sohfRQR22812%^#&FF^%@#$!sl234fl33s");

            var signinKey = new SymmetricSecurityKey(key);

            var signinCredential = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signinCredential,
                expires: DateTime.Now.AddDays(7),
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
