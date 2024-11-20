using ApiGateway.Middleware;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using BusinessLogic;
using DTO.Results.SecurityResult;
using Helper.AutoMapper;
using DTO.Results.NoteResult;
using DTO.Params.NoteParam;

namespace ApiGateway.Controllers.Users
{
    [Route("api/user/note")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserAttribute))]
    public class NoteController : ControllerBase
    {
        private readonly IAutoMap _autoMap;
        private readonly ILogic<CreateNoteParam, CreateNoteResult> _createNoteLogic;
        private readonly ILogic<UpdateNoteParam, UpdateNoteResult> _updateNoteLogic;
        private readonly ILogic<DeleteNoteParam, DeleteNoteResult> _deleteNoteLogic;
        private readonly ILogic<GetListNoteInRangeParam, GetListNoteInRangeResult> _getListNoteInRange;
        public NoteController(IAutoMap autoMap,
        ILogic<CreateNoteParam, CreateNoteResult> createNoteLogic,
        ILogic<UpdateNoteParam, UpdateNoteResult> updateNoteLogic,
        ILogic<DeleteNoteParam, DeleteNoteResult> deleteNoteLogic,
        ILogic<GetListNoteInRangeParam, GetListNoteInRangeResult> getListNoteInRange)
        {
            _autoMap = autoMap;
            _createNoteLogic = createNoteLogic;
            _updateNoteLogic = updateNoteLogic;
            _deleteNoteLogic = deleteNoteLogic;
            _getListNoteInRange = getListNoteInRange;
        }

        [HttpPost]
        public ActionResult<CreateNoteResult?> CreateNote(CreateNoteRequest request)
        {
            Log.Information($"CreateNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<CreateNoteRequest, CreateNoteParam>(request);
            param.UserId = userId;

            var returnData = _createNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPut]
        public ActionResult<UpdateNoteResult?> UpdateNote(UpdateNoteRequest request)
        {
            Log.Information($"UpdateNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<UpdateNoteRequest, UpdateNoteParam>(request);
            param.UserId = userId;

            var returnData = _updateNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<DeleteNoteResult?> DeleteNote([FromQuery] DeleteNoteRequest request)
        {
            Log.Information($"DeleteNote {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<DeleteNoteRequest, DeleteNoteParam>(request);
            param.UserId = userId;

            var returnData = _deleteNoteLogic.Execute(param)?.Result;

            return returnData;
        }

        [HttpGet]
        [Route("get-in-range")]
        public ActionResult<GetListNoteInRangeResult?> GetListNoteInRange([FromQuery] GetListNoteInRangeRequest request)
        {
            Log.Information($"GetListNoteInRange {request}");

            var userContext = HttpContext.Items["User"] as UserContextResult;
            var userId = userContext != null ? userContext.Id : "";

            var param = _autoMap.Map<GetListNoteInRangeRequest, GetListNoteInRangeParam>(request);
            param.UserId = userId;

            var returnData = _getListNoteInRange.Execute(param)?.Result;

            return returnData;
        }
    }
}
