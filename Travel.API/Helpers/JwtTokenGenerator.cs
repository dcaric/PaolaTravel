using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Travel.API.Models;

namespace Travel.API.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string Generate(ApplicationUser user)
        {

            // creating a list of claims — small pieces of identity info — that will be embedded into the JWT token.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),      // standard clainm - subject
                new Claim(JwtRegisteredClaimNames.Email, user.Email),       // standard claim - email
                new Claim("userId", user.Id.ToString()),                    // custom claim - userid
                new Claim("isAdmin", user.IsAdmin.ToString().ToLower())     // custom claim - isAdmin
            };

            // Converts secret key (from appsettings.json) into a format used for signing the token
            //  "symmetric" because the same key is used to sign and verify the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Combines the key with the signing algorithm (HMAC-SHA256). This is used to digitally sign token
            // so that the server can verify it hasn’t been tampered with.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // creates actual JWT token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );


            // serialize token from OBJECT INTO THE STRING suitable to return back to fronend (Swagger)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}