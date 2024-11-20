using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.SecurityResult
{
    public class UserContextResult
    {
        public bool Result { get; set; }
        public string Id { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}