using BusinessLogic.Logic.HangfireService.HangfireJobs;
using DTO.Params.NoteParam;
using DTO.Results.NoteResult;
using Hangfire;
using Repository.PgReposiotries.PgNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.NoteLogic
{
    public class UpdateNoteLogic : ILogic<UpdateNoteParam, UpdateNoteResult>
    {
        private readonly IPgNoteRepository _noteRepository;
        public UpdateNoteLogic(IPgNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<UpdateNoteResult>? Execute(UpdateNoteParam param)
        {
            Log.Information($"UpdateNoteLogic Param: {param}");

            var returnData = new UpdateNoteResult
            {
                Result = false,
                MsgCode = "UPDATE_NOTE_FAILED",
            };

            try
            {
                var noteUpdate = await _noteRepository.GetNoteById(param.Id);
                if (noteUpdate == null)
                {
                    returnData.MsgCode = "NOTE_NOT_FOUND";
                    return returnData;
                }


                noteUpdate.Title = param.Title;
                noteUpdate.Description = param.Description ?? "";
                noteUpdate.Color = param.Color;
                noteUpdate.FromDate = param.FromDate;
                noteUpdate.ToDate = param.ToDate;

                noteUpdate.LastModificationTime = DateTime.Now;

                var note = await _noteRepository.UpdateNote(noteUpdate);
                if (note == null)
                    return returnData;

                BackgroundJob.Schedule<PushNotificationJob>(x => x.PushNotificationAsync(note.Id.ToString()), note.FromDate.AddHours(-7));

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new UpdateNoteDataResult();

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"UpdateNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}