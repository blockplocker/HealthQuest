namespace HealthQuest.Views;

public partial class QuestDetailsPage : ContentPage
{
	public QuestDetailsPage()
	{
		InitializeComponent();
	}

    private async void OnModifyButtonClicked(object sender, EventArgs e)
    {
        var quest = ((Button)sender).BindingContext as Models.Quest;
        await Navigation.PushAsync(new QuestAdminPage(quest));
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {

    }
}