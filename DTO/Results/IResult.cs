using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results
{
    public class IResult
    {
        public bool Result { get; set; }
        public string? MsgCode { get; set; }
        public string? MsgDesc { get; set; }
    }
}