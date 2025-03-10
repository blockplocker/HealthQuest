using HealthQuest.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthQuest.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Stats _stats;
        public Stats Stats
        {
            get { return _stats; }
            set
            {
                _stats = value;
                OnPropertyChanged(nameof(Stats));
            }
        }

        private DailyQuest _dailyQuest;
        public DailyQuest DailyQuest
        {
            get { return _dailyQuest; }
            set
            {
                _dailyQuest = value;
                OnPropertyChanged(nameof(DailyQuest));
            }
        }

        public MainPageViewModel()
        {
            LoadStatsAsync();
            DailyQuest = new DailyQuest
            {
                Id = Guid.NewGuid().ToString(),
                Difficulty = Difficulty.Beginner,
                Pushups = 0,
                SitUps = 0,
                Squats = 0,
                Walk = 0,
                CurrentDay = DateTime.Today
            };
            CheckAndResetDailyQuest();
        }

        private async void LoadStatsAsync()
        {
            var statCollection = Data.DB.StatCollection();

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
                await statCollection.InsertOneAsync(Stats);
            }
        }

        private void CheckAndResetDailyQuest()
        {
            if (DailyQuest.CurrentDay != DateTime.Today)
            {
                DailyQuest.ResetDailyQuest();
                DailyQuest.CurrentDay = DateTime.Today;
            }
        }

        public async Task CompleteMissionAsync(string missionType)
        {
            bool statsChanged = false;

            switch (missionType)
            {
                case "Pushups":
                    if (DailyQuest.Pushups >= 10)
                    {
                        Stats.Strenght += DailyQuest.Pushups / 10;
                        DailyQuest.Pushups = 0;
                        statsChanged = true;
                    }
                    break;
                case "SitUps":
                    if (DailyQuest.SitUps >= 10)
                    {
                        Stats.Agility += DailyQuest.SitUps / 10;
                        DailyQuest.SitUps = 0;
                        statsChanged = true;
                    }
                    break;
                case "Squats":
                    if (DailyQuest.Squats >= 10)
                    {
                        Stats.Vigor += DailyQuest.Squats / 10;
                        DailyQuest.Squats = 0;
                        statsChanged = true;
                    }
                    break;
                case "Walk":
                    if (DailyQuest.Walk >= 5000)
                    {
                        Stats.Stamina += DailyQuest.Walk / 5000;
                        DailyQuest.Walk = 0;
                        statsChanged = true;
                    }
                    break;
            }
            if (statsChanged)
            {
                await UpdateStatsAsync();
            }
        }

        private async Task UpdateStatsAsync()
        {
            var statCollection = Data.DB.StatCollection();
            var filter = Builders<Stats>.Filter.Eq(s => s.Id, Stats.Id);
            var update = Builders<Stats>.Update
                .Set(s => s.Hp, Stats.Hp)
                .Set(s => s.Stamina, Stats.Stamina)
                .Set(s => s.Strenght, Stats.Strenght)
                .Set(s => s.Agility, Stats.Agility)
                .Set(s => s.Vigor, Stats.Vigor);

            await statCollection.UpdateOneAsync(filter, update);
        }
    }
}
