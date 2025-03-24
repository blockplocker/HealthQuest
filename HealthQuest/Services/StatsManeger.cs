using HealthQuest.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.Services
{
    public class StatsManager
    {
        private static readonly Lazy<StatsManager> _instance = new Lazy<StatsManager>(() => new StatsManager());

        public static StatsManager Instance => _instance.Value;

        public Stats Stats { get; private set; }

        private StatsManager()
        {
            LoadStatsAsync();
        }
        private async void LoadStatsAsync()
        {
            var statCollection = Data.DB.GetStatCollection();

            Stats = statCollection.Find(_ => true).SingleOrDefault();
            if (Stats == null)
            {
                Stats = new Stats
                {
                    Id = Guid.NewGuid().ToString(),
                    Hp = 100,
                    Stamina = 10,
                    Strenght = 10,
                    Agility = 10,
                    Vigor = 10
                };
                await Data.DB.InsertStatsAsync(Stats);
            }
        }

        public async Task SaveStatsAsync()
        {
            await Data.DB.ReplaceStatsAsync(Stats);
        }
    }
}
