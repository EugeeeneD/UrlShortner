using AutoMapper;
using Domain.Dto.Request;
using Domain.Interfaces;
using Domain.Models;
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

namespace Service.Repository
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User? _user;

        public UserAuthenticationRepository(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signinCredentials = GetSigninCredential();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");

            var token = new JwtSecurityToken(
                issuer: jwtConfig["validIssuer"],
                audience: jwtConfig["validAudience"],
                claims: claims,
                // change back to minutes
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(jwtConfig["expiresIn"])),
                signingCredentials: signinCredentials
                );

            return token;
        }

        private SigningCredentials GetSigninCredential()
        {
            var jwtConfig = _configuration.GetSection("JwtConfig");

            var secretKey = jwtConfig["secret"];

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secret key is null.");

            var key = Encoding.UTF8.GetBytes(secretKey);
            SymmetricSecurityKey secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // delete
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim("id", _user.Id.ToString())
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public async Task<IdentityResult> RegisterUserAsync(User userToRegister, UserRegistrationDto userToRegistrate)
        {
            var result = await _userManager.CreateAsync(userToRegister, userToRegistrate.Password);
            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.Username);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            return result;
        }
    }
}
