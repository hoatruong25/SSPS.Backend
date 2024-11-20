using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Results.UserResult
{
    public class GetListCategoryResult : IResult
    {
        public List<GetListCategoryDataResult>? Data { get; set; }
    }

    public class GetListCategoryDataResult
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
    }
}