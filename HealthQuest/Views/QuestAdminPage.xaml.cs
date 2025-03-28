using HealthQuest.Models;
using HealthQuest.Services;
using MongoDB.Driver;

namespace HealthQuest.Views;

public partial class QuestAdminPage : ContentPage
{
    private readonly IQuestFactory _questFactory;
    public Models.Quest Quest { get; set; }

    public QuestAdminPage(Models.Quest quest)
    {
        InitializeComponent();
        _questFactory = new QuestFactory();

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
            DeleteButton.IsVisible = true;
        }
        else
        {   
        // start values
        RepsStepper.Value = 10;
        RepsLabel.Text = "10";
        RepsDoneStepper.Value = 0;
        RepsDoneLabel.Text = "0";
        StatPicker.SelectedItem = "Strength";
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
            Quest = _questFactory.CreateQuest(
                Name.Text,
                Description.Text,
                (int)RepsStepper.Value,
                (int)RepsDoneStepper.Value,
                StatPicker.SelectedItem.ToString()
            );
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

    private async void OnClickedDeleteButton(object sender, EventArgs e)
    {
        if (Quest != null)
        {
            await Data.DB.DeleteQuestAsync(Quest.Id);
            await Navigation.PopAsync();
        }
    }
}
