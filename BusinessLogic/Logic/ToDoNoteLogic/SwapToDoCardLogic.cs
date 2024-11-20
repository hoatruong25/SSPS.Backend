using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class SwapToDoCardLogic : ILogic<SwapToDoCardParam, SwapToDoCardResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public SwapToDoCardLogic(IPgToDoNoteRepository toDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = toDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<SwapToDoCardResult>? Execute(SwapToDoCardParam param)
        {
            Log.Information($"SwapToDoNoteLogic Param: {param}");

            var returnData = new SwapToDoCardResult
            {
                Result = false,
                MsgCode = "SWAP_TO_DO_NOT_FAILED",
            };

            try
            {
                //var fromToDoNote = await _toDoNoteRepository.GetToDoNoteById(param.FromToDoNoteId);
                var toToDoNote = await _toDoNoteRepository.GetToDoNoteById(param.ToToDoNoteId);

                //if (fromToDoNote == null || toToDoNote == null)
                //{
                //    returnData.MsgCode = "TO_DO_NOTE_NOT_FOUND";
                //    return returnData;
                //}

                if (toToDoNote == null)
                {
                    returnData.MsgCode = "TO_DO_NOTE_NOT_FOUND";
                    return returnData;
                }

                var toDoCard = await _toDoCardRepository.GetToDoCardById(param.CardId);

                if(toDoCard?.Id == null)
                {
                    returnData.MsgCode = "TO_DO_CARD_NOT_FOUND";
                    return returnData;
                }

                toDoCard.ToDoNoteId = toToDoNote.Id;

                var card = await _toDoCardRepository.UpdateToDoCard(toDoCard);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new SwapToDoCardDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"SwapToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}