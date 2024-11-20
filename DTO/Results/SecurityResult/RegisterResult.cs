using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.SecurityResult
{
    public class RegisterResult : IResult
    {
        public RegisterDataResult? Data { get; set; }
    }
    public class RegisterDataResult
    {
        public string Id { get; set; } = null!;
    }
}