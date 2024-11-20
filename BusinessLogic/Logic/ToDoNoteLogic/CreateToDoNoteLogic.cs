using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Helper.AutoMapper;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class CreateToDoNoteLogic : ILogic<CreateToDoNoteParam, CreateToDoNoteResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        private readonly IAutoMap _autoMap;
        public CreateToDoNoteLogic(IPgToDoNoteRepository ToDoNoteRepository, IPgToDoCardRepository toDoCardRepository, IAutoMap autoMap)
        {
            _toDoNoteRepository = ToDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
            _autoMap = autoMap;
        }
        public async Task<CreateToDoNoteResult>? Execute(CreateToDoNoteParam param)
        {
            Log.Information($"CreateToDoNoteLogic Param: {param}");

            var returnData = new CreateToDoNoteResult
            {
                Result = false,
                MsgCode = "CREATE_TO_DO_NOTE_FAILED",
            };

            try
            {
                var ToDoNoteCreate = new PgToDoNote
                {
                    Title = param.Title,
                    Color = param.Color,
                    FromDate = param.FromDate,
                    ToDate = param.ToDate,

                    UserId = Guid.Parse(param.UserId),
                    CreationTime = DateTime.Now,
                    CreatorId = Guid.Parse(param.UserId),
                    IsDelete = false,
                };

                var resultData = await _toDoNoteRepository.CreateToDoNote(ToDoNoteCreate);

                if (resultData == null)
                    return returnData;

                if (param.Cards != null)
                {
                    var cards = new List<PgToDoCard>();
                    foreach ( var card in param.Cards)
                    {
                        cards.Add(new PgToDoCard
                        {
                            Id = Guid.NewGuid(),
                            ToDoNoteId = resultData.Id,
                            Description = card.Description,
                            Title = card.Title,
                        });
                    }

                    await _toDoCardRepository.CreateListToDoCard(cards);
                }

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new CreateToDoNoteDataResult
                {
                    Id = resultData.Id.ToString(),
                };

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"CreateToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}