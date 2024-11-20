using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class GetListUserParam : IParam
    {

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}