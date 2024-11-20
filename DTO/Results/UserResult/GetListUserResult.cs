using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.UserResult
{
    public class GetListUserResult : IResult
    {
        public List<GetListUserDataResult>? Data { get; set; }
        public PaginationResult? PaginationResult { get; set; }
    }

    public class GetListUserDataResult
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? School { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
    }
}