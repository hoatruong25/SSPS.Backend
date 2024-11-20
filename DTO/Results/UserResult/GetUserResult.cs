using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.UserResult
{
    public class GetUserResult : IResult
    {
        public GetUserDataResult? Data { get; set; }
    }

    public class GetUserDataResult
    {
        public string Id { get; set; } = null!;
        public string? Code { get; set; }
    }
}