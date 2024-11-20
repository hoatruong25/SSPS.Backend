using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class DeleteMoneyPlanParam : DeleteMoneyPlanRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class DeleteMoneyPlanRequest
    {
        public string MoneyPlanId { get; set; } = null!;

    }
}