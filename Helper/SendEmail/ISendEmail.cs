namespace Helper.SendEmail
{
    public interface ISendEmail
    {
        Task SendEmailOTP(string dest, string subject, string otp);
        Task SendEmailRef(string dest, string subject, string token);
    }
}