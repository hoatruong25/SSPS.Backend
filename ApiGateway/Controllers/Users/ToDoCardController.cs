using ApiGateway.Middleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BusinessLogic;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using DTO.Results.ToDoNoteResult;
using DTO.Params.ToDoNoteParam;

namespace ApiGateway.Controllers.Users
{
    [Route("api/user/to-do-card")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserAttribute))]
    public class ToDoCardController : ControllerBase
    {
        private readonly IAutoMap _autoMap;
        private readonly ILogic<CreateToDoCardParam, CreateToDoCardResult> _createToDoCardLogic;
        private readonly ILogic<UpdateToDoCardParam, UpdateToDoCardResult> _updateToDoCardLogic;
        private readonly ILogic<DeleteToDoCardParam, DeleteToDoCardResult> _deleteToDoCardLogic;
        private readonly ILogic<SwapToDoCardParam, SwapToDoCardResult> _swapToDoCardLogic;
        public ToDoCardController(IAutoMap autoMap,
        ILogic<CreateToDoCardParam, CreateToDoCardResult> createToDoCardLogic,
        ILogic<UpdateToDoCardParam, UpdateToDoCardResult> updateToDoCardLogic,
        ILogic<DeleteToDoCardParam, DeleteToDoCardResult> deleteToDoCardLogic,
        ILogic<SwapToDoCardParam, SwapToDoCardResult> swapToDoCardLogic)
        {
            _autoMap = autoMap;
            _createToDoCardLogic = createToDoCardLogic;
            _updateToDoCardLogic = updateToDoCardLogic;
            _deleteToDoCardLogic = deleteToDoCardLogic;
            _swapToDoCardLogic = swapToDoCardLogic;
        }

        [HttpPost]
        public ActionResult<CreateToDoCardResult?> CreateToDoCard(CreateToDoCardRequest request)
        {
            Log.Information($"CreateToDoCard {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<CreateToDoCardRequest, CreateToDoCardParam>(request);
            param.UserId = userId;

            var returnData = _createToDoCardLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPut]
        public ActionResult<UpdateToDoCardResult?> UpdateToDoCard(UpdateToDoCardRequest request)
        {
            Log.Information($"UpdateToDoCard {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateToDoCardRequest, UpdateToDoCardParam>(request);
            param.UserId = userId;

            var returnData = _updateToDoCardLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<DeleteToDoCardResult?> DeleteToDoCard([FromQuery] DeleteToDoCardRequest request)
        {
            Log.Information($"DeleteToDoCard {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<DeleteToDoCardRequest, DeleteToDoCardParam>(request);
            param.UserId = userId;

            var returnData = _deleteToDoCardLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpGet]
        [Route("swap")]
        public ActionResult<SwapToDoCardResult?> SwapToDoCard([FromQuery] SwapToDoCardParam param)
        {
            Log.Information($"SwapToDoCard {param}");

            var returnData = _swapToDoCardLogic.Execute(param)?.Result;

            return returnData;
        }
    }
}