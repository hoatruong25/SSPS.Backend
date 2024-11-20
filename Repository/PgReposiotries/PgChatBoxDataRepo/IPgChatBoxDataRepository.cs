using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgChatBoxDataRepo
{
    public interface IPgChatBoxDataRepository
    {
        Task CreateChatBoxData(PgChatBoxData entity);
        Task UpdateChatBoxData(PgChatBoxData entity);
        Task DeleteChatBoxData(PgChatBoxData entity);
        Task<PgChatBoxData?> GetChatBoxDataByUsageId(Guid moneyPlanId);
    }
}