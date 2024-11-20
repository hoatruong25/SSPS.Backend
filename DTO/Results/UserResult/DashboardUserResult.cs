namespace DTO.Results.UserResult
{
    public class DashboardUserResult : IResult
    {
        public DashboardUserDataResult? Data { get; set; }
    }

    public class DashboardUserDataResult
    {
        public double TotalExpectMoney { get; set; }
        public double TotalActualMoney { get; set; }
        public List<DashboardUserDiagramDataResult> ListDiagramData { get; set; } = null!;
    }

    public class DashboardUserDiagramDataResult
    {
        // Day or Month
        public double DoM { get; set; }
        public double ExpectMoney { get; set; }
        public double ActualMoney { get; set; }
    }
}
