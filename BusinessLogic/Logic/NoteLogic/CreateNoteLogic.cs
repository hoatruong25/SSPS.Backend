using BusinessLogic.Logic.HangfireService.HangfireJobs;
using DTO.Params.NoteParam;
using DTO.Results.NoteResult;
using Hangfire;
using Infrastructure.PgModels;
using Repository.PgReposiotries.PgNoteRepo;
using Serilog;

namespace BusinessLogic.Logic.NoteLogic
{
    public class CreateNoteLogic : ILogic<CreateNoteParam, CreateNoteResult>
    {
        private readonly IPgNoteRepository _noteRepository;
        public CreateNoteLogic(IPgNoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }
        public async Task<CreateNoteResult>? Execute(CreateNoteParam param)
        {
            Log.Information($"CreateNoteLogic Param: {param}");

            var returnData = new CreateNoteResult
            {
                Result = false,
                MsgCode = "CREATE_NOTE_FAILED",
            };

            try
            {
                var noteCreate = new PgNote
                {
                    Title = param.Title,
                    Description = param.Description ?? "",
                    Color = param.Color,
                    FromDate = param.FromDate,
                    ToDate = param.ToDate,

                    UserId = Guid.Parse(param.UserId),
                    CreationTime = DateTime.Now,
                    CreatorId = Guid.Parse(param.UserId),
                    IsDelete = false,
                };
                var note = await _noteRepository.CreateNote(noteCreate);

                if (note == null)
                    return returnData;

                returnData.Result = true;
                returnData.MsgCode = "SUCCESS";
                returnData.Data = new CreateNoteDataResult
                {
                    Id = note.Id.ToString(),
                };


                // Scheduled Notification for note
                //var dateTimeOffset = new DateTimeOffset(note.FromDate);
                BackgroundJob.Schedule<PushNotificationJob>(x => x.PushNotificationAsync(note.Id.ToString()), note.FromDate.AddHours(-7));

                return returnData;
            }
            catch (Exception ex)
            {
                Log.Information($"CreateNoteLogic Param: {ex}");

                returnData.MsgDesc = ex.Message;
                return returnData;
            }
        }
    }
}