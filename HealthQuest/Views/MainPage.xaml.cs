using HealthQuest.Models;
using HealthQuest.Views;

namespace HealthQuest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.MainPageViewModel();
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
                var viewModel = BindingContext as ViewModels.MainPageViewModel;

                switch (missionType)
                {
                    case "Pushups":
                        viewModel.DailyQuest.Pushups += increment;
                        break;
                    case "SitUps":
                        viewModel.DailyQuest.SitUps += increment;
                        break;
                    case "Squats":
                        viewModel.DailyQuest.Squats += increment;
                        break;
                    case "Walk":
                        viewModel.DailyQuest.Walk += increment;
                        break;
                }
            }
        }

        private void OnClickedCompleteMission(object sender, EventArgs e)
        {

        }
    }

}
