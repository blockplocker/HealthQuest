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

        public static async Task UpdateDailyQuestAsync(Models.DailyQuest dailyQuest)
        {
            var dailyQuestCollection = GetDailyQuestCollection();
            var filter = Builders<Models.DailyQuest>.Filter.Eq(dq => dq.Id, dailyQuest.Id);
            var update = Builders<Models.DailyQuest>.Update
                .Set(dq => dq.Pushups, dailyQuest.Pushups)
                .Set(dq => dq.SitUps, dailyQuest.SitUps)
                .Set(dq => dq.Squats, dailyQuest.Squats)
                .Set(dq => dq.Walk, dailyQuest.Walk)
                .Set(dq => dq.CurrentDay, dailyQuest.CurrentDay)
                .Set(dq => dq.Difficulty, dailyQuest.Difficulty);
            await dailyQuestCollection.UpdateOneAsync(filter, update);
        }

        public static async Task InsertStatsAsync(Models.Stats stats)
        {
            var statCollection = GetStatCollection();
            await statCollection.InsertOneAsync(stats);
        }

        public static async Task UpdateStatsAsync(Models.Stats stats)
        {
            var statCollection = GetStatCollection();
            var filter = Builders<Models.Stats>.Filter.Eq(s => s.Id, stats.Id);
            var update = Builders<Models.Stats>.Update
                .Set(s => s.Hp, stats.Hp)
                .Set(s => s.Stamina, stats.Stamina)
                .Set(s => s.Strenght, stats.Strenght)
                .Set(s => s.Agility, stats.Agility)
                .Set(s => s.Vigor, stats.Vigor);
            await statCollection.UpdateOneAsync(filter, update);
        }
    }
}
