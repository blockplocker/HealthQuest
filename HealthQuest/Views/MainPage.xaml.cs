using HealthQuest.Models;
using HealthQuest.Views;

namespace HealthQuest
{
    public partial class MainPage : ContentPage
    {
        private ViewModels.MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new ViewModels.MainPageViewModel();
            BindingContext = _viewModel;
        }

        private async void OnClickedTrainPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuestPage());
        }

        private void OnClickedSleepPage(object sender, EventArgs e)
        {

        }

        private void OnClickedWalkPage(object sender, EventArgs e)
        {

        }

        private void OnClickedBattlePage(object sender, EventArgs e)
        {

        }

        private void OnClickedAddMission(object sender, EventArgs e)
        {
            UpdateMission(sender, 1);
        }

        private void OnClickedRemoveMission(object sender, EventArgs e)
        {
            UpdateMission(sender, -1);
        }

        private void UpdateMission(object sender, int increment)
        {
            if (sender is Button button)
            {
                var missionType = button.CommandParameter as string;
                var dailyQuest = _viewModel.DailyQuest;
                switch (missionType)
                {
                    case "Pushups":
                        if (dailyQuest.Pushups + increment >= 0 && dailyQuest.Pushups + increment <= dailyQuest.TargetReps)
                        {
                            dailyQuest.Pushups += increment;
                        }
                        break;
                    case "SitUps":
                        if (dailyQuest.SitUps + increment >= 0 && dailyQuest.SitUps + increment <= dailyQuest.TargetReps)
                        {
                            dailyQuest.SitUps += increment;
                        }
                        break;
                    case "Squats":
                        if (dailyQuest.Squats + increment >= 0 && dailyQuest.Squats + increment <= dailyQuest.TargetReps)
                        {
                            dailyQuest.Squats += increment;
                        }
                        break;
                    case "Walk":
                        if (dailyQuest.Walk + increment >= 0 && dailyQuest.Walk + increment <= dailyQuest.TargetWalkSteps)
                        {
                            dailyQuest.Walk += increment;
                        }
                        break;
                }
            }
        }

        private async void OnClickedCompleteMission(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var missionType = button.CommandParameter as string;
                var dailyQuest = _viewModel.DailyQuest;

                bool isMissionComplete = missionType switch
                {
                    "Pushups" => !_viewModel.IsPushupsNotCompleted,
                    "SitUps" => !_viewModel.IsSitUpsNotCompleted,
                    "Squats" => !_viewModel.IsSquatsNotCompleted,
                    "Walk" => !_viewModel.IsWalkNotCompleted,
                    _ => false
                };

                if (isMissionComplete)
                {
                    await DisplayAlert("Already Completed", $"You have already completed the target {missionType.ToLower()}.", "OK");
                }
                else
                {
                    bool isTargetReached = missionType switch
                    {
                        "Pushups" => dailyQuest.Pushups >= dailyQuest.TargetReps,
                        "SitUps" => dailyQuest.SitUps >= dailyQuest.TargetReps,
                        "Squats" => dailyQuest.Squats >= dailyQuest.TargetReps,
                        "Walk" => dailyQuest.Walk >= dailyQuest.TargetWalkSteps,
                        _ => false
                    };

                    if (isTargetReached)
                    {
                        await _viewModel.CompleteMissionAsync(missionType);
                        string stat = missionType switch
                        {
                            "Pushups" => "Strength",
                            "SitUps" => "Agility",
                            "Squats" => "Vigor",
                            "Walk" => "Stamina",
                            _ => string.Empty
                        };

                        await DisplayAlert("Completed", $"You completed the target {missionType.ToLower()}.👍💪 Your {stat} has increased!", "Awesome");
                    }
                    else
                    {
                        await DisplayAlert("Incomplete", $"You have not completed the target {missionType.ToLower()}.", "OK");
                    }
                }
            }
        }
    }

}
