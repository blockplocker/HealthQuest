using HealthQuest.Services;

namespace HealthQuest.Views;

public partial class QuestPage : ContentPage
{
    private ViewModels.QuestPageViewModel _viewModel;

    public QuestPage()
    {
        InitializeComponent();
        _viewModel = new ViewModels.QuestPageViewModel();
        BindingContext = _viewModel;
    }

    private async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var quest = ((ListView)sender).SelectedItem as Models.Quest;
        if (quest != null)
        {
            await Navigation.PushAsync(new QuestAdminPage(quest));
        }
    }

    private async void OnClickedAddQuest(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.QuestAdminPage(null));
    }

    private async void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        var stepper = sender as Stepper;
        var quest = stepper?.BindingContext as Models.Quest;

        if (quest != null && quest.DayCompleted == null && quest.RepsDone >= quest.Reps)
        {
            quest.DayCompleted = DateTime.UtcNow;
            quest.RepsDone = (int)stepper.Value;
            await Data.DB.ReplaceQuestAsync(quest);

            // Update Stats using ViewModel
            await _viewModel.UpdateStatsAsync(quest);

            await DisplayAlert("Completed", $"You completed the Quest good job!!!", "Awesome");
        }
    }
}
