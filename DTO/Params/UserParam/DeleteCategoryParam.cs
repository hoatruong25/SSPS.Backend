using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class DeleteCategoryParam : DeleteCategoryRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class DeleteCategoryRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteCategoryDataRequest
    {
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
    }
}