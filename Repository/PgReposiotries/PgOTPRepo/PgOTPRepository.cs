using Infrastructure.PgModels;
using MongoDB.Driver;

namespace Repository.PgReposiotries.PgOTPRepo
{
    public class PgOTPRepository : IPgOTPRepository
    {
        private readonly PgDbContext _pgDbContext;
        public PgOTPRepository(PgDbContext pgDbContext)
        {
            _pgDbContext = pgDbContext;
        }

        public PgOTP CreateOTP(PgOTP otp)
        {
            var result = _pgDbContext.OTP.Add(otp);
            _pgDbContext.SaveChanges();

            return result.Entity;
        }

        public PgOTP? GetOTP(Guid userId, string otp, string type)
        {
            return _pgDbContext.OTP.FirstOrDefault(x => x.UserId == userId && x.OTP == otp && x.Type == type && x.IsUsed != true);
        }

        public PgOTP UpdateOTP(PgOTP otp)
        {
            var result = _pgDbContext.OTP.Update(otp);
            _pgDbContext.SaveChanges();

            return result.Entity;
        }
    }
}