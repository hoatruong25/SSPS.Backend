using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infrastructure.Models;
using MongoDB.Bson;

namespace Repository.Repositories.ToDoNoteRepo
{
    public class ToDoNoteRepository : IToDoNoteRepository
    {
        private readonly IMongoCollection<ToDoNote> _toDoNoteCollection;

        public ToDoNoteRepository(IOptions<SSPSDataSettings> sspsDataSettings)
        {
            var mongoClient = new MongoClient(sspsDataSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(sspsDataSettings.Value.DatabaseName);
            _toDoNoteCollection = mongoDatabase.GetCollection<ToDoNote>(sspsDataSettings.Value.ToDoNoteCollectionName);
        }

        public async Task<ToDoNote> CreateToDoNote(ToDoNote toDoNote)
        {
            var Id = ObjectId.GenerateNewId();
            toDoNote._id = Id;

            await _toDoNoteCollection.InsertOneAsync(toDoNote);

            return (await _toDoNoteCollection.FindAsync(x => x._id == Id)).FirstOrDefault();
        }

        public async Task<List<ToDoNote>> GetAllToDoNoteByUser(string userId)
        {
            return await _toDoNoteCollection.Find(x => x.UserId == ObjectId.Parse(userId) && x.IsDelete != true).ToListAsync();
        }

        public async Task<ToDoNote> GetToDoNoteById(string id)
        {
            return (await _toDoNoteCollection.FindAsync(x => x._id.ToString() == id && x.IsDelete == false)).FirstOrDefault();
        }

        public async Task<ToDoNote> UpdateToDoNote(ToDoNote toDoNote)
        {
            var resultData = await _toDoNoteCollection.ReplaceOneAsync(x => x._id == toDoNote._id, toDoNote);

            return _toDoNoteCollection.Find(x => x._id == toDoNote._id).FirstOrDefault();
        }
    }
}