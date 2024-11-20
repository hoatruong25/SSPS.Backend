using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class UpdateMoneyPlanParam : UpdateMoneyPlanRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class UpdateMoneyPlanRequest
    {
        public string Id { get; set; } = null!;
        // public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public List<UpdateMoneyPlanUsageParam> Usages { get; set; } = new List<UpdateMoneyPlanUsageParam>();
    }

    public class UpdateMoneyPlanUsageParam
    {
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int? Priority { get; set; }
        public string? CategoryId { get; set; }
    }
}