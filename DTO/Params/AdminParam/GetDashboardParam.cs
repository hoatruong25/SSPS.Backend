using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class GetDashboardParam : IParam
    {
        public int Year { get; set; }
    }
}