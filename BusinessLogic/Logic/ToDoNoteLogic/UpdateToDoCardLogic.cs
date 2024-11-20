using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Repository.PgReposiotries.PgToDoCardRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class UpdateToDoCardLogic : ILogic<UpdateToDoCardParam, UpdateToDoCardResult>
    {
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public UpdateToDoCardLogic(IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<UpdateToDoCardResult>? Execute(UpdateToDoCardParam param)
        {
            Log.Information($"UpdateToDoNoteLogic Param: {param}");

            var returnData = new UpdateToDoCardResult
            {
                Result = false,
                MsgCode = "UPDATE_TO_DO_CARD_FAILED",
            };

            try
            {
                var toDoCard = await _toDoCardRepository.GetToDoCardById(param.CardId);

                if (toDoCard == null)
                {
                    returnData.MsgCode = "TO_DO_CARD_NOT_FOUND";
                    return returnData;
                }

                toDoCard.Title = param.Title;
                toDoCard.Description = param.Description;

                var resultData = await _toDoCardRepository.UpdateToDoCard(toDoCard);

                if (resultData == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateToDoCardDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"UpdateToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}