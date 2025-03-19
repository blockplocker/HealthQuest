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
            RepsStepper.Value = Quest.Reps;
            RepsLabel.Text = Quest.Reps.ToString();
            RepsDoneStepper.Value = Quest.RepsDone;
            RepsDoneLabel.Text = Quest.RepsDone.ToString();
            StatPicker.SelectedItem = Quest.Stat;
            SaveButton.Text = "Update Quest";
        }
    }

    private void OnRepsStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        RepsLabel.Text = e.NewValue.ToString();
    }

    private void OnRepsDoneStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        RepsDoneLabel.Text = e.NewValue.ToString();
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
                Reps = (int)RepsStepper.Value,
                RepsDone = (int)RepsDoneStepper.Value,
                Stat = StatPicker.SelectedItem.ToString()
            };
            await Data.DB.InsertQuestAsync(Quest);
        }
        else
        {
            Quest.Name = Name.Text;
            Quest.Description = Description.Text;
            Quest.Reps = (int)RepsStepper.Value;
            Quest.RepsDone = (int)RepsDoneStepper.Value;
            Quest.Stat = StatPicker.SelectedItem.ToString();

            await Data.DB.ReplaceQuestAsync(Quest);
        }

        await Navigation.PopAsync();
    }
}
