using DTO.Params.SecurityParam;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using Helper.Security;
using Infrastructure.PgModels;
using MongoDB.Bson;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.AuthenticationLogic
{
    public class RegisterLogic : ILogic<RegisterParam, RegisterResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IJwtToken _jwtToken;
        private readonly IAutoMap _autoMap;
        public RegisterLogic(IPgUserRepository userRepository, IJwtToken jwtToken, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _jwtToken = jwtToken;
            _autoMap = autoMap;
        }

        public async Task<RegisterResult>? Execute(RegisterParam param)
        {
            Log.Information($"RegisterLogic Param: {param}");

            var returnData = new RegisterResult
            {
                Result = false,
                MsgCode = "REGISTER_FAILED",
            };

            try
            {
                var isEmailExit = await _userRepository.IsEmailExit(param.Email);
                if (isEmailExit)
                {
                    returnData.MsgDesc = "Email was exist";
                    return returnData;
                }

                var isCodeExit = await _userRepository.IsCodeExit(param.Email);
                if (isCodeExit)
                {
                    returnData.MsgDesc = "Code was exist";
                    return returnData;
                }

                var userCreate = _autoMap.Map<RegisterParam, PgUser>(param);

                userCreate.Id = Guid.NewGuid();
                userCreate.Password = BCrypt.Net.BCrypt.HashPassword(param.Password);
                userCreate.CreationTime = DateTime.Now;

                var user = _userRepository.CreateUser(userCreate);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new RegisterDataResult
                {
                    Id = userCreate.Id.ToString()
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"RegisterLogic Error: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}