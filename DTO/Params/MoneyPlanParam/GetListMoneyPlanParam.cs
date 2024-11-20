using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class GetListMoneyPlanParam : GetListMoneyPlanRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class GetListMoneyPlanRequest
    {
        // public string? Type { get; set; } // Day, month, year
        public string? FromDate { get; set; } // Format yyyy-MM-dd
        public string? ToDate { get; set; }
    }
}