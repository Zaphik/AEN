using CommunityToolkit.Mvvm.Input;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.MenuViewModels.GameViewModels;

public partial class PickGameViewModel : BasePageViewModel
{
    // Navigates to the new game view
    [RelayCommand]
    private void NavToNewGame()
    {
        GameViewModel.NextNav?.Invoke(2);
    }

    // Navigates to the load game view
    [RelayCommand]
    private void NavToLoadGame()
    {
        LoadGameViewModel.GetSave();
        GameViewModel.NextNav?.Invoke(1);
    }

    // Exits the application
    [RelayCommand]
    private void Exit()
    {
        System.Environment.Exit(0);
    }
}