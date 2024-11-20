using DTO.Const;
using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class UpdateUserLogic : ILogic<UpdateUserParam, UpdateUserResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        public UpdateUserLogic(IPgUserRepository userRepository, IAutoMap autoMap)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
        }
        public async Task<UpdateUserResult>? Execute(UpdateUserParam param)
        {
            Log.Information($"UpdateUserLoigc Param: {param}");

            var returnData = new UpdateUserResult
            {
                Result = false,
                MsgCode = "GET_USER_FAILED",
            };

            try
            {
                var user = await _userRepository.GetUser(param.Id);

                if (user == null)
                {
                    returnData.MsgCode = "USER_NOT_FOUND";
                    return returnData;
                }

                user.FirstName = param.FirstName ?? user.FirstName;
                user.LastName = param.LastName ?? user.LastName;
                // Need change logic
                user.Phone = param.Phone ?? user.Phone;
                user.School = param.School ?? user.School;
                user.Location = param.Location ?? user.Location;
                user.LastModificationTime = DateTime.Now;

                if (param.Status != null)
                {
                    // Logic change Status
                    switch (user.Status?.ToUpper())
                    {
                        case UserConst.USER_STATUS_ACTIVE:
                            if (param.Status?.ToUpper() != UserConst.USER_STATUS_BLOCK)
                            {
                                returnData.MsgCode = "INVALID_STATUS";
                                return returnData;
                            }
                            user.Status = param.Status.ToUpper();
                            break;

                        case UserConst.USER_STATUS_BLOCK:
                            if (param.Status == UserConst.USER_STATUS_DELETE)
                            {
                                user.IsDelete = true;
                                user.DeletionTime = DateTime.Now;
                                user.DeletorId = Guid.Parse(param.UpdatedBy);
                            }
                            if (param.Status != UserConst.USER_STATUS_ACTIVE || param.Status != UserConst.USER_STATUS_DELETE)
                            {
                                returnData.MsgCode = "INVALID_STATUS";
                                return returnData;
                            }
                            user.Status = param.Status.ToUpper();
                            break;

                        case UserConst.USER_STATUS_DELETE:
                            returnData.MsgCode = "USER_WAS_DELETED";
                            return returnData;
                        default:
                            break;
                    }
                }

                var result = await _userRepository.UpdateUser(user);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateUserDataResult();

                return returnData;

            }
            catch (Exception ex)
            {
                Log.Information($"UpdateUserLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}