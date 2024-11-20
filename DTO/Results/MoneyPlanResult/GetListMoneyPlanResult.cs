namespace DTO.Results.MoneyPlanResult
{
    public class GetListMoneyPlanResult : IResult
    {
        public List<GetListMoneyPlanDataResult>? Data { get; set; }
        public PaginationResult? PaginationResult { get; set; }
    }

    public class GetListMoneyPlanDataResult
    {
        public string Id { get; set; }
        // public string Type { get; set; } = null!; // Day, month, year
        public string Status { get; set; } = null!;
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public DateTime Date { get; set; }
        public string? CurrencyUnit { get; set; }
        public List<GetListMoneyPlanDataUsageMoneyResult>? UsageMoneys { get; set; }
    }

    public class GetListMoneyPlanDataUsageMoneyResult
    {
        public string? Name { get; set; }
        public double ExpectAmount { get; set; }
        public double ActualAmount { get; set; }
        public string? CategoryName { get; set; }
        public int? Priority { get; set; }
    }
}
