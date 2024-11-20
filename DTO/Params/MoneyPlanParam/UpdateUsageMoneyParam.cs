using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class UpdateUsageMoneyParam : UpdateUsageMoneyRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class UpdateUsageMoneyRequest
    {
        public string? MoneyPlanId { get; set; }
        public List<UpdateUsageMoneyDataParam> Data { get; set; } = new List<UpdateUsageMoneyDataParam>();
    }

    public class UpdateUsageMoneyDataParam
    {
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int? Priority { get; set; }
        public string? CategoryId { get; set; }
    }
}