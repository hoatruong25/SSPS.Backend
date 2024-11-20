namespace DTO.Results.MoneyPlanResult
{
    public class CreateMoneyPlanResult : IResult
    {
        public CreateMoneyPlanDataResult? Data { get; set; }
    }

    public class CreateMoneyPlanDataResult
    {
        public string? Id { get; set; }
    }
}
