using ApiGateway.Middleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using DTO.Params.UserParam;
using DTO.Results.UserResult;
using BusinessLogic;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using DTO.Results.AdminResult;
using DTO.Params.MoneyPlanParam;

namespace ApiGateway.Controllers.Admin
{
    [Route("api/admin/user")]
    [ApiController]
    //[TypeFilter(typeof(AuthorizeAdminAttribute))]
    public class UserController : ControllerBase
    {
        private readonly ILogic<GetListUserParam, GetListUserResult> _getListUserLogic;
        private readonly ILogic<GetUserDetailParam, GetUserDetailResult> _getUserDetailLogic;
        private readonly ILogic<UpdateUserParam, UpdateUserResult> _updateUserLogic;
        private readonly ILogic<GetDashboardParam, GetDashboardResult> _getDashboardLogic;
        private readonly IAutoMap _autoMap;
        public UserController(
            ILogic<GetListUserParam, GetListUserResult> getListUserLogic,
            ILogic<GetUserDetailParam, GetUserDetailResult> getUserDetailLogic,
            ILogic<UpdateUserParam, UpdateUserResult> updateUserLogic,
            ILogic<GetDashboardParam, GetDashboardResult> getDashboardLogic,
            IAutoMap autoMap)
        {
            _getListUserLogic = getListUserLogic;
            _getUserDetailLogic = getUserDetailLogic;
            _updateUserLogic = updateUserLogic;
            _getDashboardLogic = getDashboardLogic;
            _autoMap = autoMap;
        }

        [HttpGet]
        public ActionResult<GetListUserResult> GetListUser([FromQuery] GetListUserParam param)
        {
            Log.Information($"GetListUser {param}");

            var returnData = _getListUserLogic.Execute(param)?.Result;

            if (returnData == null)
                return new GetListUserResult
                {
                    Result = false,
                    MsgCode = "GET_LIST_USER_FAILED"
                };

            return returnData;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<GetUserDetailResult> GetUserDetail([FromRoute] string id)
        {
            Log.Information($"GetUserDetail {id}");

            var returnData = _getUserDetailLogic.Execute(new GetUserDetailParam { Id = id })?.Result;

            if (returnData == null)
                return new GetUserDetailResult
                {
                    Result = false,
                    MsgCode = "GET_USER_FAILED",
                };

            return returnData;
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<UpdateUserResult>> UpdateUser([FromRoute] string id, UpdateUserRequest request)
        {
            Log.Information($"UpdateUser {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateUserRequest, UpdateUserParam>(request);
            param.UpdatedBy = userId;
            param.Id = id;

            var returnData = await _updateUserLogic.Execute(param);

            if (returnData == null)
                return new UpdateUserResult
                {
                    Result = false,
                    MsgCode = "GET_USER_FAILED",
                };

            return returnData;
        }

        [HttpPost]
        [Route("dashboard")]
        public async Task<ActionResult<GetDashboardResult>> GetDashboard([FromQuery] int year)
        {
            Log.Information($"GetDashboard {year}");

            var returnData = await _getDashboardLogic.Execute(new GetDashboardParam
            {
                Year = year
            });

            if (returnData == null)
                return new GetDashboardResult
                {
                    Result = false,
                    MsgCode = "GET_DASHBOARD_FAILED",
                };

            return returnData;
        }
    }
}
