using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class UpdateToDoNoteLogic : ILogic<UpdateToDoNoteParam, UpdateToDoNoteResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public UpdateToDoNoteLogic(IPgToDoNoteRepository toDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = toDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<UpdateToDoNoteResult>? Execute(UpdateToDoNoteParam param)
        {
            Log.Information($"UpdateToDoNoteLogic Param: {param}");

            var returnData = new UpdateToDoNoteResult
            {
                Result = false,
                MsgCode = "UPDATE_TO_DO_NOTE_FAILED",
            };

            try
            {
                var ToDoNoteUpdate = await _toDoNoteRepository.GetToDoNoteById(param.Id);
                if (ToDoNoteUpdate == null)
                {
                    returnData.MsgCode = "TO_DO_NOTE_NOT_FOUND";
                    return returnData;
                }


                ToDoNoteUpdate.Title = param.Title;
                ToDoNoteUpdate.Color = param.Color;
                ToDoNoteUpdate.FromDate = param.FromDate;
                ToDoNoteUpdate.ToDate = param.ToDate;

                ToDoNoteUpdate.LastModificationTime = DateTime.Now;

                var resultData = await _toDoNoteRepository.UpdateToDoNote(ToDoNoteUpdate);

                if (resultData == null)
                    return returnData;

                await _toDoCardRepository.DeleteToDoCardByToDoNoteId(resultData.Id.ToString());

                if (param.Cards != null)
                {
                    var cards = new List<PgToDoCard>();
                    foreach(var card in param.Cards)
                    {
                        cards.Add(new PgToDoCard
                        {
                            ToDoNoteId = resultData.Id,
                            Title = card.Title,
                            Description = card.Description,
                        });
                    }

                    await _toDoCardRepository.CreateListToDoCard(cards);
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateToDoNoteDataResult();

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