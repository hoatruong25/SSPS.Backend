using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class CreateToDoCardLogic : ILogic<CreateToDoCardParam, CreateToDoCardResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public CreateToDoCardLogic(IPgToDoNoteRepository ToDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = ToDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<CreateToDoCardResult>? Execute(CreateToDoCardParam param)
        {
            Log.Information($"CreateToDoCardLogic Param: {param}");

            var returnData = new CreateToDoCardResult
            {
                Result = false,
                MsgCode = "CREATE_TO_DO_CARD_FAILED",
            };

            try
            {
                var toDoNote = await _toDoNoteRepository.GetToDoNoteById(param.ToDoNoteId);

                if (toDoNote == null)
                {
                    returnData.MsgCode = "TO_DO_NOTE_NOT_FOUND";
                    return returnData;
                }

                var ToDoCardCreate = new PgToDoCard
                {
                    Id = Guid.NewGuid(),
                    ToDoNoteId = toDoNote.Id,
                    Title = param.Card.Title,
                    Description = param.Card.Description,
                };

                var resultData = await _toDoCardRepository.CreateToDoCard(ToDoCardCreate);

                if (resultData == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new CreateToDoCardDataResult
                {
                    Id = resultData.Id.ToString(),
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"CreateToDoCardLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}