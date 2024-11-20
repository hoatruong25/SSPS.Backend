using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class GetListCategoryParam : IParam
    {
        public string UserId { get; set; } = null!;
    }
}