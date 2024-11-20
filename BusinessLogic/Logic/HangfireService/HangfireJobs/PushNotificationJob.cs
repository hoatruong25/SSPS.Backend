using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helper.FirebaseNoti;
using Repository.PgReposiotries.PgNoteRepo;
using Repository.PgReposiotries.PgUserRepo;

namespace BusinessLogic.Logic.HangfireService.HangfireJobs
{
    public class PushNotificationJob
    {
        private readonly INotificationService _notificationService;
        private readonly IPgNoteRepository _noteRepository;
        private readonly IPgUserRepository _userRepository;
        public PushNotificationJob(INotificationService notificationService, IPgNoteRepository noteRepository, IPgUserRepository userRepository)
        {
            _notificationService = notificationService;
            _noteRepository = noteRepository;
            _userRepository = userRepository;
        }
        public async Task PushNotificationAsync(string noteId)
        {
            var note = await _noteRepository.GetNoteById(noteId);

            // Check note is null
            if (note == null) return;

            var startTime = note.FromDate.AddMinutes(-1);
            var endTime = note.FromDate.AddMinutes(1);

            var now = DateTime.Now;

            // Check note was updated
            if (startTime > now || endTime < now) return;

            // Get device token
            var user = await _userRepository.GetUser(note.UserId.ToString());

            if (user == null) return;

            var deviceToken = user.DeviceToken ?? "";

            await _notificationService.SendNotification(note.Title, note.Description, deviceToken, new Dictionary<string, string>());
        }
    }
}