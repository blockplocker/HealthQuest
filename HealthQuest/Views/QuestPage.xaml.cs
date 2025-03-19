namespace HealthQuest.Views;

public partial class QuestPage : ContentPage
{
    public QuestPage()
    {
        InitializeComponent();
        BindingContext = new ViewModels.QuestPageViewModel();
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
}
