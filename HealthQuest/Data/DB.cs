using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Data
{
    internal class DB
    {
        private static MongoClient GetClient()
        {
            const string connectionUri = "mongodb+srv://noadb:Hemligt123@cluster0.tyg2x.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            return client;
        }

        public static IMongoCollection<Models.Stats> GetStatCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase("MyStatsDb");
            var statCollection = database.GetCollection<Models.Stats>("MyStats");
            return statCollection;
        }

        public static IMongoCollection<Models.DailyQuest> GetDailyQuestCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase("MyDailyQuestDb");
            var dailyQuestCollection = database.GetCollection<Models.DailyQuest>("MyDailyQuests");
            return dailyQuestCollection;
        }

        public static async Task InsertDailyQuestAsync(Models.DailyQuest dailyQuest)
        {
            var dailyQuestCollection = GetDailyQuestCollection();
            await dailyQuestCollection.InsertOneAsync(dailyQuest);
        }

        public static async Task ReplaceDailyQuestAsync(Models.DailyQuest dailyQuest)
        {
            var dailyQuestCollection = GetDailyQuestCollection();
            var filter = Builders<Models.DailyQuest>.Filter.Eq(dq => dq.Id, dailyQuest.Id);
            await dailyQuestCollection.ReplaceOneAsync(filter, dailyQuest);
        }

        public static async Task InsertStatsAsync(Models.Stats stats)
        {
            var statCollection = GetStatCollection();
            await statCollection.InsertOneAsync(stats);
        }

        public static async Task ReplaceStatsAsync(Models.Stats stats)
        {
            var statCollection = GetStatCollection();
            var filter = Builders<Models.Stats>.Filter.Eq(s => s.Id, stats.Id);
            await statCollection.ReplaceOneAsync(filter, stats);
        }
    }
}
