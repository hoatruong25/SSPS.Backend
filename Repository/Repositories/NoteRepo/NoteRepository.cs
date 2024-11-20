using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infrastructure.Models;
using MongoDB.Bson;

namespace Repository.Repositories.NoteRepo
{
    public class NoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _noteCollection;

        public NoteRepository(IOptions<SSPSDataSettings> sspsDataSettings)
        {
            var mongoClient = new MongoClient(sspsDataSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(sspsDataSettings.Value.DatabaseName);
            _noteCollection = mongoDatabase.GetCollection<Note>(sspsDataSettings.Value.NoteCollectionName);
        }

        public async Task<Note> CreateNote(Note note)
        {
            var Id = ObjectId.GenerateNewId();
            note._id = Id;

            await _noteCollection.InsertOneAsync(note);

            return (await _noteCollection.FindAsync(x => x._id == Id)).FirstOrDefault();
        }

        public async Task<List<Note>> GetListNoteInRange(string userId, DateTime fromDate, DateTime toDate)
        {
            var resultData = await _noteCollection.Find(x => x.UserId == ObjectId.Parse(userId) && x.IsDelete != true && ((x.FromDate.Date >= fromDate.Date && x.FromDate.Date <= toDate.Date) || (x.ToDate.Date >= fromDate.Date && x.ToDate.Date <= toDate.Date))).ToListAsync();

            return resultData;
        }

        public async Task<Note?> GetNoteById(string id)
        {
            var resultData = (await _noteCollection.FindAsync(x => x._id.ToString() == id && x.IsDelete == false)).FirstOrDefault();
            return resultData;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            var resultData = await _noteCollection.ReplaceOneAsync(x => x._id == note._id, note);

            return _noteCollection.Find(x => x._id == note._id).FirstOrDefault();
        }

        public async Task<List<int>> GetTotalNoteEachMonth(int year)
        {
            var notes = (await _noteCollection.FindAsync(x => x.IsDelete == false && x.CreationTime.Value.Year == year)).ToList();
            var resultData = new List<int>
            {
                notes.Where(x => x.CreationTime.Value.Month == 1).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 2).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 3).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 4).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 5).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 6).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 7).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 8).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 9).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 10).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 11).Count(),
                notes.Where(x => x.CreationTime.Value.Month == 12).Count(),
            };

            return resultData;
        }
    }
}