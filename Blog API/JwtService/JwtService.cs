using Blog_API.Identity;
using Blog_API.Modules.Users.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
                new Claim(ClaimTypes.NameIdentifier , user.Email!.ToString()),
                new Claim(ClaimTypes.Email , user.Email.ToString()),// in ClaimType adding Email of user in jwt payload(optional)
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
                Token = token , 
                Username = user.UserName,
                Email = user.Email,
                TokenExpirationDateTime = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["RefreshToken:Expires"]))
            };
        }

        public string GenerateRefreshToken ()
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal? GetClaimsPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false //should Be false!!! this action beacuse refreshtoken
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256 , StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }
            return claimsPrincipal;
        }
    }   
}
