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

        public static IMongoCollection<Models.Stats> StatCollection()
        {
            var client = GetClient();
            var database = client.GetDatabase("MyStatsDb");
            var statCollection = database.GetCollection<Models.Stats>("MyStats");
            return statCollection;
        }
    }
}
