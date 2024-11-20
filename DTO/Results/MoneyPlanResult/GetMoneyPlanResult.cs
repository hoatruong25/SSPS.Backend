using System.Diagnostics.Contracts;

namespace DTO.Results.MoneyPlanResult
{
    public class GetMoneyPlanResult : IResult
    {
        public GetMoneyPlanDataResult? Data { get; set; }
        public PaginationResult? PaginationResult { get; set; }
    }

    public class GetMoneyPlanDataResult
    {
        public string Id { get; set; }
        // public string Type { get; set; } = null!; // Day, month, year
        public string Status { get; set; } = null!;
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public string? CurrencyUnit { get; set; }
        public List<GetMoneyPlanDataUsageMoneyResult>? UsageMoneys { get; set; }
    }

    public class GetMoneyPlanDataUsageMoneyResult
    {
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public int Priority { get; set; } = 0;
        public string? CategoryName { get; set; }
    }
}
