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
                OnPropertyChanged(nameof(IsPushupsNotCompleted));
                OnPropertyChanged(nameof(IsSitUpsNotCompleted));
                OnPropertyChanged(nameof(IsSquatsNotCompleted));
                OnPropertyChanged(nameof(IsWalkNotCompleted));
            }
        }

        public bool IsPushupsNotCompleted => DailyQuest.Pushups < DailyQuest.TargetReps;
        public bool IsSitUpsNotCompleted => DailyQuest.SitUps < DailyQuest.TargetReps;
        public bool IsSquatsNotCompleted => DailyQuest.Squats < DailyQuest.TargetReps;
        public bool IsWalkNotCompleted => DailyQuest.Walk < DailyQuest.TargetWalkSteps;

        private Weather _weather;
        public Weather Weather
        {
            get { return _weather; }
            set
            {
                _weather = value;
                OnPropertyChanged(nameof(Weather));
            }
        }
        public MainPageViewModel()
        {
            LoadStatsAsync();
            LoadDailyQuestAsync();
            LoadWeatherAsync();
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

        private async void LoadDailyQuestAsync()
        {
            var dailyQuestCollection = Data.DB.GetDailyQuestCollection();

            DailyQuest = dailyQuestCollection.Find(_ => true).SingleOrDefault();
            if (DailyQuest == null)
            {
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
                await Data.DB.InsertDailyQuestAsync(DailyQuest);
            }
            else
            {
                CheckAndResetDailyQuest();
            }
        }
        private async void LoadWeatherAsync()
        {
            var location = await  Services.GeoLocation.GetCurrentLocationAsync();
            if (location != null)
            {
                string uri = $"v1/weather?lat={location.Latitude}&lon={location.Longitude}";
                Weather = await WeatherService.GetWeatherAsync(uri);
            }
        }

        private void CheckAndResetDailyQuest()
        {
            if (DailyQuest.CurrentDay != DateTime.Today)
            {
                DailyQuest.ResetDailyQuest();
                DailyQuest.CurrentDay = DateTime.Today;
                _ = Data.DB.ReplaceDailyQuestAsync(DailyQuest);
                OnPropertyChanged(nameof(IsPushupsNotCompleted));
                OnPropertyChanged(nameof(IsSitUpsNotCompleted));
                OnPropertyChanged(nameof(IsSquatsNotCompleted));
                OnPropertyChanged(nameof(IsWalkNotCompleted));
            }
        }

        public async Task CompleteMissionAsync(string missionType)
        {

            switch (missionType)
            {
                case "Pushups":
                        Stats.Strenght += DailyQuest.Pushups / 10;
                    break;
                case "SitUps":
                        Stats.Agility += DailyQuest.SitUps / 10;
                    break;
                case "Squats":
                        Stats.Vigor += DailyQuest.Squats / 10;
                    break;
                case "Walk":
                        Stats.Stamina += DailyQuest.Walk / 5000;
                    break;
            }
            
                await Data.DB.ReplaceStatsAsync(Stats);
                await Data.DB.ReplaceDailyQuestAsync(DailyQuest);
                OnPropertyChanged(nameof(IsPushupsNotCompleted));
                OnPropertyChanged(nameof(IsSitUpsNotCompleted));
                OnPropertyChanged(nameof(IsSquatsNotCompleted));
                OnPropertyChanged(nameof(IsWalkNotCompleted));
            
        }
    }
}
