using DTO.Params.NoteParam;
using DTO.Results.NoteResult;
using Repository.PgReposiotries.PgNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.NoteLogic
{
    public class DeleteNoteLogic : ILogic<DeleteNoteParam, DeleteNoteResult>
    {
        private readonly IPgNoteRepository _noteRepository;
        public DeleteNoteLogic(IPgNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<DeleteNoteResult>? Execute(DeleteNoteParam param)
        {
            Log.Information($"DeleteNoteLogic Param: {param}");

            var returnData = new DeleteNoteResult
            {
                Result = false,
                MsgCode = "DELETE_NOTE_FAILED",
            };

            try
            {
                var noteDelete = await _noteRepository.GetNoteById(param.Id);
                if (noteDelete == null)
                {
                    returnData.MsgCode = "NOTE_NOT_FOUND";
                    return returnData;
                }


                noteDelete.IsDelete = true;
                noteDelete.DeletionTime = DateTime.Now;
                noteDelete.DeletorId = Guid.Parse(param.UserId);

                noteDelete.LastModificationTime = DateTime.Now;

                var user = await _noteRepository.UpdateNote(noteDelete);

                if (user == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new DeleteNoteDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"DeleteNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}