using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgOTPRepo;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class ActiveAccountOTPLogic : ILogic<ActiveAccountOTPParam, ActiveAccountOTPResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        private readonly IPgOTPRepository _pgOTPRepository;
        public ActiveAccountOTPLogic(IPgUserRepository userRepository, IAutoMap autoMap, IPgOTPRepository pgOTPRepository)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
            _pgOTPRepository = pgOTPRepository;
        }

        public async Task<ActiveAccountOTPResult>? Execute(ActiveAccountOTPParam param)
        {
            Log.Information($"RegisterOTPLogic Param: {param}");

            var returnData = new ActiveAccountOTPResult
            {
                Result = false,
                MsgCode = "ACTIVE_ACCOUNT_OTP",
            };

            try
            {
                var user = await _userRepository.GetUserByEmailToActive(param.Email);

                if (user == null)
                {
                    returnData.MsgDesc = "Username not found";
                    return returnData;
                }

                var otpRecord = _pgOTPRepository.GetOTP(user.Id, param.OTP, "REGISTER");

                if (otpRecord == null)
                {
                    returnData.MsgDesc = "OTP not found";
                    return returnData;
                }

                otpRecord.IsUsed = true;
                otpRecord.UseDate = DateTime.Now;

                _pgOTPRepository.UpdateOTP(otpRecord);

                user.Status = "ACTIVE";

                await _userRepository.UpdateUser(user);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new ActiveAccountOTPDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"ActiveAccountOTPLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}