using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class GetUserParam : IParam
    {
        public string Id { get; set; } = null!;
    }
}