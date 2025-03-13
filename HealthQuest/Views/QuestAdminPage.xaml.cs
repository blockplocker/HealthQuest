using HealthQuest.Models;

namespace HealthQuest.Views;

public partial class QuestAdminPage : ContentPage
{
    public Models.Quest Quest { get; set; }
    public QuestAdminPage(Models.Quest quest)
	{
		InitializeComponent();

        Quest = quest;

        if (Quest != null)
        {
            Name.Text = Quest.Name;
            Description.Text = Quest.Description;
            Reps.Text = Quest.Reps.ToString();
            RepsDone.Text = Quest.RepsDone.ToString();
            Stat.Text = Quest.Stat.ToString();
            SaveButton.Text = "Update Quest";
        }
	}

    private void OnCreateOrUpdateButtonClicked(object sender, EventArgs e)
    {

    }
}