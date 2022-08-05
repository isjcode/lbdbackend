using lbdbackend.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace lbdbackend.Service.Services {
    public class JWTManager : IJWTManager {
        private readonly UserManager<IdentityUser> _userManager;

        public JWTManager(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }
        public async Task<string> GenerateToken(IdentityUser user) {
            List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

            IList<string> roles = await _userManager.GetRolesAsync(user);

            foreach (string role in roles) {
                Claim claim = new Claim(ClaimTypes.Role, role);

                claims.Add(claim);
            }

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("31f10115-f47d-4fca-897e-f2afd7a2aa3f"));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    issuer: "",
                    audience: "",
                    claims: claims,
                    expires: DateTime.Now.AddHours(1)
                );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        }

        public string GetUsernameByToken(string token) {
            return new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.ToList().FirstOrDefault(e => e.Type == ClaimTypes.Name).Value;
        }
    }
}
