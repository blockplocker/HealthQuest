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
                Pushups = 0,
                SitUps = 0,
                Squats = 0,
                Walk = 0
            };
        }

        private async Task LoadStatsAsync()
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
    }
}
