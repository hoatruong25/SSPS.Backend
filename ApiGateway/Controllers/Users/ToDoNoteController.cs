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
    [Route("api/user/to-do-note")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserAttribute))]
    public class ToDoNoteController : ControllerBase
    {
        private readonly IAutoMap _autoMap;
        private readonly ILogic<CreateToDoNoteParam, CreateToDoNoteResult> _createToDoNoteLogic;
        private readonly ILogic<UpdateToDoNoteParam, UpdateToDoNoteResult> _updateToDoNoteLogic;
        private readonly ILogic<DeleteToDoNoteParam, DeleteToDoNoteResult> _deleteToDoNoteLogic;
        private readonly ILogic<GetAllToDoNoteParam, GetAllToDoNoteResult> _getAllToDoNoteLogic;
        private readonly ILogic<GetToDoNoteParam, GetToDoNoteResult> _getToDoNoteLogic;
        public ToDoNoteController(IAutoMap autoMap,
        ILogic<CreateToDoNoteParam, CreateToDoNoteResult> createToDoNoteLogic,
        ILogic<UpdateToDoNoteParam, UpdateToDoNoteResult> updateToDoNoteLogic,
        ILogic<DeleteToDoNoteParam, DeleteToDoNoteResult> deleteToDoNoteLogic,
        ILogic<GetAllToDoNoteParam, GetAllToDoNoteResult> getAllToDoNoteLogic,
        ILogic<GetToDoNoteParam, GetToDoNoteResult> getToDoNoteLogic)
        {
            _autoMap = autoMap;
            _createToDoNoteLogic = createToDoNoteLogic;
            _updateToDoNoteLogic = updateToDoNoteLogic;
            _deleteToDoNoteLogic = deleteToDoNoteLogic;
            _getAllToDoNoteLogic = getAllToDoNoteLogic;
            _getToDoNoteLogic = getToDoNoteLogic;
        }

        [HttpPost]
        public ActionResult<CreateToDoNoteResult?> CreateToDoNote(CreateToDoNoteRequest request)
        {
            Log.Information($"CreateToDoNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<CreateToDoNoteRequest, CreateToDoNoteParam>(request);
            param.UserId = userId;

            var returnData = _createToDoNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPut]
        public ActionResult<UpdateToDoNoteResult?> UpdateToDoNote(UpdateToDoNoteRequest request)
        {
            Log.Information($"UpdateToDoNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateToDoNoteRequest, UpdateToDoNoteParam>(request);
            param.UserId = userId;

            var returnData = _updateToDoNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<DeleteToDoNoteResult?> DeleteToDoNote([FromQuery] DeleteToDoNoteRequest request)
        {
            Log.Information($"DeleteToDoNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<DeleteToDoNoteRequest, DeleteToDoNoteParam>(request);
            param.UserId = userId;

            var returnData = _deleteToDoNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpGet]
        [Route("get-all")]
        public ActionResult<GetAllToDoNoteResult?> GetAllToDoNote()
        {

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            Log.Information($"GetAllToDoNote {userId}");

            var param = new GetAllToDoNoteParam
            {
                UserId = userId,
            };

            var returnData = _getAllToDoNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<GetToDoNoteResult?> GetAllToDoNote([FromRoute] string id)
        {

            Log.Information($"GetToDoNote {id}");

            var param = new GetToDoNoteParam
            {
                Id = id,
            };

            var returnData = _getToDoNoteLogic.Execute(param)?.Result;

            return returnData;
        }
    }
}
