using HealthQuest.ViewModels;

namespace HealthQuest.Views;

public partial class BattlePage : ContentPage
{
    public BattlePage()
    {
        InitializeComponent();
        BindingContext = new BattlePageViewModel();
    }
}
