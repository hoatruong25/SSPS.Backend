namespace DTO.Params.MoneyPlanParam
{
    public class CreateListMoneyPlanParam : CreateListMoneyPlanRequest, IParam
    {
        public string UserId { get; set; } = null!;
    }

    public class CreateListMoneyPlanRequest
    {
        public double ExpectAmount { get; set; }
        public string? CurrencyUnit { get; set; }
        public string FromDate { get; set; } = null!;
        public string ToDate { get; set; } = null!;
        public List<CreateMoneyPlanUsageMoneyRequest>? UsageMoneys { get; set; }
    }
}