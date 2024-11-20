using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class GetToDoNoteLogic : ILogic<GetToDoNoteParam, GetToDoNoteResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public GetToDoNoteLogic(IPgToDoNoteRepository toDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = toDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<GetToDoNoteResult>? Execute(GetToDoNoteParam param)
        {
            Log.Information($"GetToDoNoteLogic Param: {param}");

            var returnData = new GetToDoNoteResult
            {
                Result = false,
                MsgCode = "GET_TO_DO_NOTE_FAILED",
            };

            try
            {
                var toDoNotes = await _toDoNoteRepository.GetToDoNoteById(param.Id);
                returnData.Data = _autoMap.Map<PgToDoNote, GetToDoNoteDataResult>(toDoNotes);

                var cards = await _toDoCardRepository.GetToDocardByToDoNoteId(toDoNotes.Id);
                returnData.Data.Cards = _autoMap.Map<List<PgToDoCard>, List<GetToDoNoteCardDataResult>>(cards);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}