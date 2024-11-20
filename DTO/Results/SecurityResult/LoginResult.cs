using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.SecurityResult
{
    public class LoginResult : IResult
    {
        public LoginDataResult? Data { get; set; }
    }
    public class LoginDataResult
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}