namespace HealthQuest.Views;

public partial class QuestDetailsPage : ContentPage
{
    private Models.Quest _quest;

    public QuestDetailsPage(Models.Quest quest)
    {
        InitializeComponent();
        _quest = quest;
        BindingContext = _quest;

        RepsDoneLabel.Text = _quest.RepsDone.ToString();
        RepsDoneStepper.Maximum = _quest.Reps;
        RepsDoneStepper.Value = _quest.RepsDone;
        UpdateCompletionStatus();
    }

    private async void OnRepsDoneStepperValueChanged(object sender, ValueChangedEventArgs e)
    {
        _quest.RepsDone = (int)e.NewValue;
        RepsDoneLabel.Text = _quest.RepsDone.ToString();
        UpdateCompletionStatus();
        await Data.DB.UpdateRepsDoneAsync(_quest.Id, _quest.RepsDone);
    }

    private void UpdateCompletionStatus()
    {
        if (_quest.RepsDone >= _quest.Reps)
        {
            CompletionStatusLabel.Text = "Complete";
            CompletionStatusLabel.TextColor = Colors.Green;
        }
        else
        {
            CompletionStatusLabel.Text = "Incomplete";
            CompletionStatusLabel.TextColor = Colors.Red;
        }
    }

    private async void OnModifyButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuestAdminPage(_quest));
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Implement delete functionality here
    }
}
