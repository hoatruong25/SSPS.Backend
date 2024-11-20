using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class UpdateUserParam : UpdateUserRequest, IParam
    {
        public string Id { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
    }

    public class UpdateUserRequest
    {
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? School { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
    }
}