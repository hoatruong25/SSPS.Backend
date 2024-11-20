using DTO.Params.UserParam;
using DTO.Results.UserResult;
using Helper.AutoMapper;
using Helper.FirebaseNoti;
using Repository.PgReposiotries.PgUserRepo;
using Serilog;

namespace BusinessLogic.Logic.UserLogic
{
    public class GetListCategoryLogic : ILogic<GetListCategoryParam, GetListCategoryResult>
    {
        private readonly IPgUserRepository _userRepository;
        private readonly IAutoMap _autoMap;
        private readonly INotificationService _notificationService;
        public GetListCategoryLogic(IPgUserRepository userRepository, IAutoMap autoMap, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _autoMap = autoMap;
            _notificationService = notificationService;
        }
        public async Task<GetListCategoryResult>? Execute(GetListCategoryParam param)
        {
            Log.Information($"GetListCategoryLogic Param: {param}");

            var returnData = new GetListCategoryResult
            {
                Result = false,
                MsgCode = "GET_LIST_CATEGORY_FAILED",
            };

            try
            {
                var categories = await _userRepository.GetListCategoryByUserId(param.UserId);

                //_ = _notificationService.SendNotification();

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = categories.Select(x => new GetListCategoryDataResult
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    IsDefault = x.IsDefault
                }).ToList();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetListCategoryLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}