namespace DTO.Params.MoneyPlanParam
{
    public class CreateMoneyPlanParam : CreateMoneyPlanRequest, IParam
    {
        public string? UserId { get; set; }
    }

    public class CreateMoneyPlanRequest
    {
        public string Type { get; set; } = null!;
        public double? ExpectAmount { get; set; }
        public string? CurrencyUnit { get; set; }
        public DateTime DateTime { get; set; }
        public List<CreateMoneyPlanUsageMoneyRequest>? UsageMoneys { get; set; }
    }

    public class CreateMoneyPlanUsageMoneyRequest
    {
        public string? Name { get; set; }
        public double? ExpectAmount { get; set; }
        public int? Priority { get; set; }
        public string? CategoryId { get; set; }
    }
}