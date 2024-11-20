using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.SecurityParam
{
    public class ChangePasswordParam : ChangePasswordRequest, IParam
    {
        public string Id { get; set; } = null!;
    }

    public class ChangePasswordRequest
    {
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password must include at least 1 uppercase character, 1 number and 1 special character!")]
        public string CurrentPassword { get; set; } = null!;
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password must include at least 1 uppercase character, 1 number and 1 special character!")]
        public string NewPassword { get; set; } = null!;
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password must include at least 1 uppercase character, 1 number and 1 special character!")]
        public string ConfirmPassword { get; set; } = null!;
    }
}