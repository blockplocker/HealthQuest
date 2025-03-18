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
        var Quest = ((ListView)sender).SelectedItem as Models.Quest;
        if (Quest != null)
        {
            var page = new QuestDetailsPage();
            page.BindingContext = Quest;
            await Navigation.PushAsync(page);
        }
    }

    private async void OnClickedAddQuest(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.QuestAdminPage(null));
    }
}
