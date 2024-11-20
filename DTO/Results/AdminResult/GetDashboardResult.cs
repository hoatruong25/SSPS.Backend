using DTO.Results.ToDoNoteResult;

namespace DTO.Results.AdminResult
{
    public class GetDashboardResult : IResult
    {
        public GetDashboardDataResult Data { get; set; }
    }

    public class GetDashboardDataResult
    {
        public int AmountOfUser { get; set; }
        // In Year
        public List<int> AmountOfNewUser { get; set; } = new List<int>();
        public List<int> AmountOfNote { get; set; } = new List<int>();
        public List<int> AmountOfMoneyPlan { get; set; } = new List<int>();
    }
}
