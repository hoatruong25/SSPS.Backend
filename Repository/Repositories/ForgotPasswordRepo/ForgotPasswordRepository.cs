using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Infrastructure.Models;
using MongoDB.Bson;

namespace Repository.Repositories.ForgotPasswordRepo
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly IMongoCollection<ForgotPassword> _forgotPasswordCollection;
        public ForgotPasswordRepository(IOptions<SSPSDataSettings> sspsDataSettings)
        {
            var mongoClient = new MongoClient(sspsDataSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(sspsDataSettings.Value.DatabaseName);
            _forgotPasswordCollection = mongoDatabase.GetCollection<ForgotPassword>(sspsDataSettings.Value.ForgotPasswordCollectionName);
        }

        public async Task<ForgotPassword> CreateForgotPasswordAsync(ForgotPassword forgotPassword)
        {
            var Id = ObjectId.GenerateNewId();
            forgotPassword._id = Id;

            await _forgotPasswordCollection.InsertOneAsync(forgotPassword);

            return (await _forgotPasswordCollection.FindAsync(x => x._id == Id)).FirstOrDefault();
        }

        public async Task<ForgotPassword?> GetForgotPasswordByTokenAsync(string token)
        {
            var resultData = (await _forgotPasswordCollection.FindAsync(x => x.Token == token)).FirstOrDefault();
            return resultData;
        }

        public async Task<bool> UpdateForgotPassword(ForgotPassword forgotPassword)
        {
            await _forgotPasswordCollection.ReplaceOneAsync(x => x._id == forgotPassword._id, forgotPassword);
            return true;
        }
    }
}