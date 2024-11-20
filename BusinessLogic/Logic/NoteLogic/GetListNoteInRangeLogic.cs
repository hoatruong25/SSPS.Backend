using DTO.Params.NoteParam;
using DTO.Results.NoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.NoteLogic
{
    public class GetListNoteInRangeLogic : ILogic<GetListNoteInRangeParam, GetListNoteInRangeResult>
    {
        private readonly IPgNoteRepository _noteRepository;
        private readonly IAutoMap _autoMap;
        public GetListNoteInRangeLogic(IPgNoteRepository noteRepository, IAutoMap autoMap)
        {
            _noteRepository = noteRepository;
            _autoMap = autoMap;
        }
        public async Task<GetListNoteInRangeResult>? Execute(GetListNoteInRangeParam param)
        {
            Log.Information($"GetListNoteInRangeLogic Param: {param}");

            var returnData = new GetListNoteInRangeResult
            {
                Result = false,
                MsgCode = "GET_LIST_NOTE_FAILED",
            };

            try
            {
                var notes = await _noteRepository.GetListNoteInRange(param.UserId, param.FromDate, param.ToDate);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = _autoMap.Map<List<PgNote>, List<GetListNoteInRangeDataResult>>(notes);

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetListNoteInRangeLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}