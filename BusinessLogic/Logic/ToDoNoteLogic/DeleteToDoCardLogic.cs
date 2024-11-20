using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Repository.PgReposiotries.PgToDoCardRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class DeleteToDoCardLogic : ILogic<DeleteToDoCardParam, DeleteToDoCardResult>
    {
        private readonly IPgToDoCardRepository _toDoCardRepository;
        public DeleteToDoCardLogic(IPgToDoCardRepository toDoCardRepository)
        {
            _toDoCardRepository = toDoCardRepository;
        }
        public async Task<DeleteToDoCardResult>? Execute(DeleteToDoCardParam param)
        {
            Log.Information($"DeleteToDoCardLogic Param: {param}");

            var returnData = new DeleteToDoCardResult
            {
                Result = false,
                MsgCode = "DELETE_TO_DO_CARD_FAILED",
            };

            try
            {
                await _toDoCardRepository.DeleteToDoCard(param.CardId);

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new DeleteToDoCardDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"DeleteToDoCardLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}