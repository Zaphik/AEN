using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.MGModel;
using Panda.Server;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.MenuViewModels.GameViewModels;

public partial class NewGameViewModel : BasePageViewModel
{
    [ObservableProperty] private Save selectedSave;

    public ObservableCollection<Save> SaveChoices { get; }

    public NewGameViewModel()
    {
        // Sets the save choices
        SaveChoices = new ObservableCollection<Save>
        {
            new("Natsu"),
            new("Erza")
        };
        SelectedSave = SaveChoices[0];
    }

    [RelayCommand]
    private async Task StartGame()
    {
        // if empty, gets settings from jsonctrl
        GameState.Settings ??= JsonCTRL.GetSettings()?[0];

        // if selected save is not null, sets the save and score to the selected save
        if (SelectedSave.Choice != null)
        {
            // if user id is not null, sends the values to sqlctrl
            if (UiGlobals.UserID != null)
            {
                GameState.Save = new Save
                {
                    SaveID = await SqlCTRL.SaveGame(SelectedSave.Choice, UiGlobals.UserID)!,
                    Choice = SelectedSave.Choice
                };
                GameState.Score = new Score
                {
                    ScoreID = await SqlCTRL.SaveScore((int)UiGlobals.UserID, GameState.Save.Choice)
                };
            }
            else
            {
                GameState.Save = new Save { Choice = SelectedSave.Choice };
                GameState.Score = new Score() { Character = SelectedSave.Choice };
            }
        }

        // starts the game
        using var game = new Pandemonium();
        game.Run();
    }

    //navigate back
    [RelayCommand]
    private void Back()
    {
        GameViewModel.NextNav?.Invoke(0);
    }
}