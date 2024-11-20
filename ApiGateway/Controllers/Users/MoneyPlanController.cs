using ApiGateway.Middleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BusinessLogic;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using DTO.Results.MoneyPlanResult;
using DTO.Params.MoneyPlanParam;

namespace ApiGateway.Controllers.Users
{
    [Route("api/user/money-plan")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserAttribute))]
    public class MoneyPlanController : ControllerBase
    {
        private readonly IAutoMap _autoMap;
        private readonly ILogic<CreateMoneyPlanParam, CreateMoneyPlanResult> _createMoneyPlanLogic;
        private readonly ILogic<GetMoneyPlanParam, GetMoneyPlanResult> _getMoneyPlanLogic;
        private readonly ILogic<GetListMoneyPlanParam, GetListMoneyPlanResult> _getListMoneyPlanLogic;
        private readonly ILogic<UpdateUsageMoneyParam, UpdateUsageMoneyResult> _updateUsageMoneyPlanLogic;
        private readonly ILogic<UpdateMoneyPlanParam, UpdateMoneyPlanResult> _updateMoneyPlanLogic;
        private readonly ILogic<DeleteMoneyPlanParam, DeleteMoneyPlanResult> _deleteMoneyPlanLogic;
        private readonly ILogic<CreateListMoneyPlanParam, CreateListMoneyPlanResult> _createListMoneyPlanLogic;
        public MoneyPlanController(IAutoMap autoMap, ILogic<CreateMoneyPlanParam, CreateMoneyPlanResult> createMoneyPlanLogic, ILogic<GetMoneyPlanParam, GetMoneyPlanResult> getMoneyPlanLogic, ILogic<GetListMoneyPlanParam, GetListMoneyPlanResult> getListMoneyPlanLogic, ILogic<UpdateUsageMoneyParam, UpdateUsageMoneyResult> updateUsageMoneyPlanLogic, ILogic<UpdateMoneyPlanParam, UpdateMoneyPlanResult> updateMoneyPlanLogic, ILogic<DeleteMoneyPlanParam, DeleteMoneyPlanResult> deleteMoneyPlanLogic, ILogic<CreateListMoneyPlanParam, CreateListMoneyPlanResult> createListMoneyPlanLogic)
        {
            _autoMap = autoMap;
            _createMoneyPlanLogic = createMoneyPlanLogic;
            _getMoneyPlanLogic = getMoneyPlanLogic;
            _getListMoneyPlanLogic = getListMoneyPlanLogic;
            _updateMoneyPlanLogic = updateMoneyPlanLogic;
            _updateUsageMoneyPlanLogic = updateUsageMoneyPlanLogic;
            _deleteMoneyPlanLogic = deleteMoneyPlanLogic;
            _createListMoneyPlanLogic = createListMoneyPlanLogic;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<GetMoneyPlanResult?> GetMoneyPlan([FromRoute] string id)
        {
            Log.Information($"GetMoneyPlan {id}");

            var returnData = _getMoneyPlanLogic.Execute(new GetMoneyPlanParam
            {
                Id = id
            })?.Result;

            return returnData;
        }

        [HttpPost]
        public ActionResult<CreateMoneyPlanResult?> CreateMoneyPlan(CreateMoneyPlanRequest request)
        {
            Log.Information($"CreateMoneyPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<CreateMoneyPlanRequest, CreateMoneyPlanParam>(request);
            param.UserId = userId;

            var returnData = _createMoneyPlanLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("create-list-money-plan")]
        public ActionResult<CreateListMoneyPlanResult?> CreateListMoneyPlan(CreateListMoneyPlanRequest request)
        {
            Log.Information($"CreateListMoneyPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<CreateListMoneyPlanRequest, CreateListMoneyPlanParam>(request);
            param.UserId = userId;

            var returnData = _createListMoneyPlanLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpGet]
        [Route("range-type")]
        public ActionResult<GetListMoneyPlanResult?> GetListMoneyPlan([FromQuery] GetListMoneyPlanRequest request)
        {
            Log.Information($"GetListMoneyPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<GetListMoneyPlanRequest, GetListMoneyPlanParam>(request);
            param.UserId = userId;

            var returnData = _getListMoneyPlanLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("update-money-plan")]
        public ActionResult<UpdateMoneyPlanResult?> UpdateMoneyPlanPlan(UpdateMoneyPlanRequest request)
        {
            Log.Information($"UpdateMoneyPlanPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateMoneyPlanRequest, UpdateMoneyPlanParam>(request);
            param.UserId = userId;

            var returnData = _updateMoneyPlanLogic.Execute(param)?.Result;
            return returnData;
        }

        [HttpPost]
        [Route("update-usage-money-plan")]
        public ActionResult<UpdateUsageMoneyResult?> UpdateUsageMoneyPlan(UpdateUsageMoneyRequest request)
        {
            Log.Information($"UpdateUsageMoneyPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = new UpdateUsageMoneyParam
            {
                UserId = userId,
                MoneyPlanId = request.MoneyPlanId,
                Data = request.Data
            };

            var returnData = _updateUsageMoneyPlanLogic.Execute(param)?.Result;
            return returnData;
        }

        [HttpPost]
        [Route("delete-money-plan")]
        public ActionResult<DeleteMoneyPlanResult?> DeleteMoneyPlanPlan([FromQuery] DeleteMoneyPlanRequest request)
        {
            Log.Information($"DeleteMoneyPlanPlan {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = new DeleteMoneyPlanParam
            {
                UserId = userId,
                MoneyPlanId = request.MoneyPlanId,
            };

            var returnData = _deleteMoneyPlanLogic.Execute(param)?.Result;
            return returnData;
        }
    }
}
