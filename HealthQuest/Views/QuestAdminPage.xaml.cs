using HealthQuest.Models;
using MongoDB.Driver;

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

    private async void OnClickedSaveButton(object sender, EventArgs e)
    {
        if (Quest == null)
        {
            Quest = new Models.Quest()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name.Text,
                Description = Description.Text,
                Reps = int.Parse(Reps.Text),
                RepsDone = int.Parse(RepsDone.Text),
                Stat = Stat.Text
            };
            await Data.DB.InsertQuestAsync(Quest);
        }
        else
        {
            Quest.Name = Name.Text;
            Quest.Description = Description.Text;
            Quest.Reps = int.Parse(Reps.Text);
            Quest.RepsDone = int.Parse(RepsDone.Text);
            Quest.Stat = Stat.Text;

            await Data.DB.ReplaceQuestAsync(Quest);
        }

        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]); // remove the quest details from the nav stack
        await Navigation.PopAsync();
    }
}
