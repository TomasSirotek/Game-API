using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Identity.Entities;
using Microsoft.IdentityModel.Tokens;

namespace API.Configuration {
    public class JWToken : IJWToken  {
        
        private readonly IConfiguration _config;
    

        public JWToken (IConfiguration config)
        {
            _config = config;
        }
        
        public string CreateToken(AppUser user)
        {
            List<Claim> claims = new () {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes(
                _config.GetSection("JwtConfig:Secret").Value));

            var credentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken (
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler ().WriteToken (token);

            return jwt;
        }

    }
}