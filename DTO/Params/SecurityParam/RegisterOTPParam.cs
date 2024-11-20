using System.ComponentModel.DataAnnotations;
using DTO.Const;

namespace DTO.Params.SecurityParam
{
    public class RegisterOTPParam : IParam
    {
        public string Code { get; set; } = null!;
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address (e.g. @gmail.com)")]
        public string Email { get; set; } = null!;
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).*$", ErrorMessage = "Password must include at least 1 uppercase character, 1 number and 1 special character!")]
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = UserConst.USER_ROLE_USER;
        public string? School { get; set; }
        public string? Location { get; set; }
    }
}