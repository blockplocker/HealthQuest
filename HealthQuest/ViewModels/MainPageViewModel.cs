using HealthQuest.Models;
using HealthQuest.Services;
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
        private readonly IQuestFactory _questFactory;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Stats Stats => StatsManager.Instance.Stats;

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

        private bool _isPushupsNotCompleted = true;
        public bool IsPushupsNotCompleted
        {
            get { return _isPushupsNotCompleted; }
            private set
            {
                _isPushupsNotCompleted = value;
                OnPropertyChanged(nameof(IsPushupsNotCompleted));
            }
        }

        private bool _isSitUpsNotCompleted = true;
        public bool IsSitUpsNotCompleted
        {
            get { return _isSitUpsNotCompleted; }
            private set
            {
                _isSitUpsNotCompleted = value;
                OnPropertyChanged(nameof(IsSitUpsNotCompleted));
            }
        }

        private bool _isSquatsNotCompleted = true;
        public bool IsSquatsNotCompleted
        {
            get { return _isSquatsNotCompleted; }
            private set
            {
                _isSquatsNotCompleted = value;
                OnPropertyChanged(nameof(IsSquatsNotCompleted));
            }
        }

        private bool _isWalkNotCompleted = true;
        public bool IsWalkNotCompleted
        {
            get { return _isWalkNotCompleted; }
            private set
            {
                _isWalkNotCompleted = value;
                OnPropertyChanged(nameof(IsWalkNotCompleted));
            }
        }

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
            _questFactory = new QuestFactory();
            LoadDailyQuestAsync();
            LoadWeatherAsync();
        }

        private async void LoadWeatherAsync()
        {
            var location = await Services.GeoLocation.GetCurrentLocationAsync();
            if (location != null)
            {
                string uri = $"v1/weather?lat={location.Latitude}&lon={location.Longitude}";
                Weather = await WeatherService.GetWeatherAsync(uri);
            }
        }

        private async void LoadDailyQuestAsync()
        {
            var dailyQuestCollection = Data.DB.GetDailyQuestCollection();

            DailyQuest = dailyQuestCollection.Find(_ => true).SingleOrDefault();
            if (DailyQuest == null)
            {
                DailyQuest = _questFactory.CreateDailyQuest(Difficulty.Beginner);
                await Data.DB.InsertDailyQuestAsync(DailyQuest);
            }
            else
            {
                CheckAndResetDailyQuest();
            }
        }

        private void CheckAndResetDailyQuest()
        {
            if (DailyQuest.CurrentDay.ToUniversalTime().Date != DateTime.UtcNow.Date)
            {
                DailyQuest.ResetDailyQuest();
                DailyQuest.CurrentDay = DateTime.Today;
                _ = Data.DB.ReplaceDailyQuestAsync(DailyQuest);
            }

            IsPushupsNotCompleted = DailyQuest.Pushups < DailyQuest.TargetReps;
            IsSitUpsNotCompleted = DailyQuest.SitUps < DailyQuest.TargetReps;
            IsSquatsNotCompleted = DailyQuest.Squats < DailyQuest.TargetReps;
            IsWalkNotCompleted = DailyQuest.Walk < DailyQuest.TargetWalkSteps;
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

            await StatsManager.Instance.SaveStatsAsync();
            await Data.DB.ReplaceDailyQuestAsync(DailyQuest);

            IsPushupsNotCompleted = DailyQuest.Pushups < DailyQuest.TargetReps;
            IsSitUpsNotCompleted = DailyQuest.SitUps < DailyQuest.TargetReps;
            IsSquatsNotCompleted = DailyQuest.Squats < DailyQuest.TargetReps;
            IsWalkNotCompleted = DailyQuest.Walk < DailyQuest.TargetWalkSteps;
        }
    }
}
