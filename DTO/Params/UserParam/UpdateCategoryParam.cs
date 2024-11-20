using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class UpdateCategoryParam : UpdateCategoryRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class UpdateCategoryRequest
    {
        public List<UpdateCategoryDataRequest> Categories { get; set; } = new List<UpdateCategoryDataRequest>();
    }

    public class UpdateCategoryDataRequest
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool? IsDefault { get; set; }
    }
}