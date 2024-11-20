using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Params.UserParam
{
    public class DashboardUserParam : DashboardUserRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class DashboardUserRequest
    {
        public string Type { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
