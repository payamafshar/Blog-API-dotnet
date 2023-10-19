using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog_API.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            DateTime expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:Expires"]));

            //Token Payload
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),//subject userId(required)
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()), // JWT unique Id(required)
                new Claim(JwtRegisteredClaimNames.Iat , DateTime.UtcNow.ToString()), // Issued At -> date and time token generation(required)
                new Claim(ClaimTypes.NameIdentifier , user.UserName!.ToString()), // in ClaimType adding username of user in jwt payload(optional)
            };
            //Token Security Key(better to save in Env variables) Type Byte 
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );
            //Chosing Algortim of Token With passing Security key to SignInCredentials
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Creating Token with Token Generator
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                 signingCredentials:signingCredentials
                );
            //Writing Token
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(tokenGenerator);

            return new AuthenticationResponse()
            {
                token = token , 
                Username = user.UserName,
                Email = user.Email
            };
        }
    }   
}
