using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.SecurityParam
{
    public class LoginParam : IParam
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address (e.g. @gmail.com)")]
        public string? Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password must include at least 1 uppercase character, 1 number and 1 special character!")]
        public string? Password { get; set; }
        public string? DeviceToken { get; set; }
    }
}