using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.SecurityResult
{
    public class RefreshTokenResult : IResult
    {
        public RefreshTokenDataResult? Data { get; set; }
    }
    public class RefreshTokenDataResult
    {
        public string? AccessToken { get; set; }
    }
}