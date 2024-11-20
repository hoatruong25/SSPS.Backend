using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.SecurityParam
{
    public class ForgotPasswordParam : IParam
    {
        public string Email { get; set; } = null!;
    }
}