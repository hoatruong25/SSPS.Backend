using DTO.Results.SecurityResult;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Helper.Security
{
    public class JwtToken : IJwtToken
    {
        public JwtToken()
        {

        }

        public AuthTokenUserResult GenerateJwtToken(string id, string code, string email, string role, string firstName, string lastName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.TokenAuthSecretKey);
            var key = Encoding.ASCII.GetBytes("SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", id),
                    new Claim("code", code),
                    new Claim("role", role),
                    new Claim("email", email),
                    new Claim("firstName", firstName),
                    new Claim("lastName", lastName),
                }),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", id),
                    new Claim("role", role),
                    new Claim("expires", new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeSeconds().ToString())
                        }),
                Expires = DateTime.Now.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return new AuthTokenUserResult
            {
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = tokenHandler.WriteToken(refreshToken),
                Expires = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeSeconds(), // token will expire after 24 hours unit by second
                Id = id,
                Role = role,
                Result = token != null && refreshToken != null ? true : false,
            };
        }

        public AuthTokenUserResult? VerifyToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_appSettings.TokenAuthSecretKey);
                var key = Encoding.ASCII.GetBytes("SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@SSPS@2023@");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "id");
                var role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role");
                return new AuthTokenUserResult
                {
                    AccessToken = token,
                    Id = id != null ? id.Value : "",
                    Role = role != null ? role.Value : ""
                };
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
