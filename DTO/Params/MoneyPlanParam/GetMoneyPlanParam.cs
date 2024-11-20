using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DTO.Params.MoneyPlanParam
{
    public class GetMoneyPlanParam : IParam
    {
        public string? Id { get; set; }
    }
}