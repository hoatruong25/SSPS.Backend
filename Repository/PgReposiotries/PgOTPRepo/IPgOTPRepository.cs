using Infrastructure.PgModels;

namespace Repository.PgReposiotries.PgOTPRepo
{
    public interface IPgOTPRepository
    {
        PgOTP CreateOTP(PgOTP otp);
        PgOTP UpdateOTP(PgOTP otp);
        PgOTP? GetOTP(Guid userId, string otp, string type);
    }
}