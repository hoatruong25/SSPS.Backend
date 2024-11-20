

using DTO.Results.SecurityResult;

namespace Helper.Security
{
    public interface IJwtToken
    {
        AuthTokenUserResult GenerateJwtToken(string id, string code, string email, string role, string firstName, string lastName);
        AuthTokenUserResult? VerifyToken(string token);

    }
}
