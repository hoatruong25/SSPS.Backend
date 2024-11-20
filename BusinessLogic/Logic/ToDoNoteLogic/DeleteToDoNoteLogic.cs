using DTO.Params.ToDoNoteParam;
using DTO.Results.ToDoNoteResult;
using Repository.PgReposiotries.PgToDoCardRepo;
using Repository.PgReposiotries.PgToDoNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.ToDoNoteLogic
{
    public class DeleteToDoNoteLogic : ILogic<DeleteToDoNoteParam, DeleteToDoNoteResult>
    {
        private readonly IPgToDoNoteRepository _toDoNoteRepository;
        private readonly IPgToDoCardRepository _toDoCardRepository;
        public DeleteToDoNoteLogic(IPgToDoNoteRepository toDoNoteRepository, IPgToDoCardRepository toDoCardRepository)
        {
            _toDoNoteRepository = toDoNoteRepository;
            _toDoCardRepository = toDoCardRepository;
        }
        public async Task<DeleteToDoNoteResult>? Execute(DeleteToDoNoteParam param)
        {
            Log.Information($"DeleteToDoNoteLogic Param: {param}");

            var returnData = new DeleteToDoNoteResult
            {
                Result = false,
                MsgCode = "DELETE_TO_DO_NOTE_FAILED",
            };

            try
            {
                var ToDoNoteDelete = await _toDoNoteRepository.GetToDoNoteById(param.Id);
                if (ToDoNoteDelete == null)
                {
                    returnData.MsgCode = "TO_DO_NOTE_NOT_FOUND";
                    return returnData;
                }


                ToDoNoteDelete.IsDelete = true;
                ToDoNoteDelete.DeletionTime = DateTime.Now;
                ToDoNoteDelete.DeletorId = Guid.Parse(param.UserId);

                ToDoNoteDelete.LastModificationTime = DateTime.Now;

                var user = await _toDoNoteRepository.UpdateToDoNote(ToDoNoteDelete);

                if (user == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new DeleteToDoNoteDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"DeleteToDoNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}