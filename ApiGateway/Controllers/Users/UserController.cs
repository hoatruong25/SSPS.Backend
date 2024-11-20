using ApiGateway.Middleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BusinessLogic;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using DTO.Results.MoneyPlanResult;
using DTO.Params.MoneyPlanParam;
using DTO.Params.UserParam;
using DTO.Results.UserResult;

namespace ApiGateway.Controllers.Users
{
    [Route("api/user")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserAttribute))]
    public class UserController : ControllerBase
    {
        private readonly IAutoMap _autoMap;
        private readonly ILogic<GetListCategoryParam, GetListCategoryResult> _getListCategoryLogic;
        private readonly ILogic<UpdateCategoryParam, UpdateCategoryResult> _updateCategoryLogic;
        private readonly ILogic<DeleteCategoryParam, DeleteCategoryResult> _deleteCategoryLogic;
        private readonly ILogic<GetUserDetailParam, GetUserDetailResult> _getUserDetailLogic;
        private readonly ILogic<UpdateUserParam, UpdateUserResult> _updateUserLogic;
        private readonly ILogic<DashboardUserParam, DashboardUserResult> _dashboardUserLogic;
        public UserController(IAutoMap autoMap,
        ILogic<GetListCategoryParam, GetListCategoryResult> getListCategoryLogic,
        ILogic<UpdateCategoryParam, UpdateCategoryResult> updateCategoryLogic,
        ILogic<DeleteCategoryParam, DeleteCategoryResult> deleteCategoryLogic,
        ILogic<GetUserDetailParam, GetUserDetailResult> getUserDetailLogic,
        ILogic<UpdateUserParam, UpdateUserResult> updateUserLogic,
        ILogic<DashboardUserParam, DashboardUserResult> dashboardUserLogic)
        {
            _autoMap = autoMap;
            _getListCategoryLogic = getListCategoryLogic;
            _updateCategoryLogic = updateCategoryLogic;
            _deleteCategoryLogic = deleteCategoryLogic;
            _getUserDetailLogic = getUserDetailLogic;
            _updateUserLogic = updateUserLogic;
            _dashboardUserLogic = dashboardUserLogic;
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

        [HttpGet]
        [Route("category")]
        public ActionResult<GetListCategoryResult?> GetListCategory()
        {
            Log.Information($"GetListCategory");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = new GetListCategoryParam
            {
                UserId = userId,
            };

            var returnData = _getListCategoryLogic.Execute(param)?.Result;
            return returnData;
        }

        [HttpPost]
        [Route("update-category")]
        public ActionResult<UpdateCategoryResult?> UpdateCategory(UpdateCategoryRequest request)
        {
            Log.Information($"UpdateCategory");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateCategoryRequest, UpdateCategoryParam>(request);
            param.UserId = userId;

            var returnData = _updateCategoryLogic.Execute(param)?.Result;
            return returnData;
        }

        [HttpPost]
        [Route("delete-category")]
        public ActionResult<DeleteCategoryResult?> DeleteCategory(DeleteCategoryRequest request)
        {
            Log.Information($"DeleteCategory");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<DeleteCategoryRequest, DeleteCategoryParam>(request);
            param.UserId = userId;

            var returnData = _deleteCategoryLogic.Execute(param)?.Result;
            return returnData;
        }

        [HttpGet]
        [Route("dashboard-user")]
        public ActionResult<DashboardUserResult?> DashboardUser([FromQuery] DashboardUserRequest request)
        {
            Log.Information($"DashboardUser");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<DashboardUserRequest, DashboardUserParam>(request);
            param.UserId = userId;

            var returnData = _dashboardUserLogic.Execute(param)?.Result;
            return returnData;
        }
    }
}
