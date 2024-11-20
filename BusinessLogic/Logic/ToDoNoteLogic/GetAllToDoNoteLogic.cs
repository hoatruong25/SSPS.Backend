using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class GetAllToDoNoteLogic : ILogic<GetAllToDoNoteParam, GetAllToDoNoteResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public GetAllToDoNoteLogic(IPgToDoNoteRepository toDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = toDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<GetAllToDoNoteResult>? Execute(GetAllToDoNoteParam param)
        {
            Log.Information($"GetAllToDoNoteLogic Param: {param}");

            var returnData = new GetAllToDoNoteResult
            {
                Result = false,
                MsgCode = "GET_ALL_TO_DO_NOTE_FAILED",
            };

            try
            {
                var ToDoNotes = await _toDoNoteRepository.GetAllToDoNoteByUser(param.UserId);
                returnData.Data = _autoMap.Map<List<PgToDoNote>, List<GetAllToDoNoteDataResult>>(ToDoNotes);

                foreach (var item in returnData.Data)
                {
                    var cards = await _toDoCardRepository.GetToDocardByToDoNoteId(Guid.Parse(item.Id));
                    item.Cards = _autoMap.Map<List<PgToDoCard>, List<GetAllToDoNoteCardDataResult>>(cards);
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"GetAllToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}