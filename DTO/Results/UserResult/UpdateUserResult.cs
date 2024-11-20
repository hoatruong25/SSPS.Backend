using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.UserResult
{
    public class UpdateUserResult : IResult
    {
        public UpdateUserDataResult? Data { get; set; }
    }

    public class UpdateUserDataResult
    {

    }
}