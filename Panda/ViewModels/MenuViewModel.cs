using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels;

namespace Panda.ViewModels;

// Follows the same page pattern as mainwindowviewmodel
public partial class MenuViewModel : BasePageViewModel
{
    [ObservableProperty] private BasePageViewModel currentPage;

    private readonly ObservableCollection<BasePageViewModel> pages;

    public MenuViewModel(IServiceProvider SERVICES)
    {
        pages =
        [
            SERVICES.GetRequiredService<GameViewModel>(),
            SERVICES.GetRequiredService<settingsViewModel>(),
            SERVICES.GetRequiredService<LeaderBoardViewModel>()
        ];
        CurrentPage = pages[0];
    }

    [RelayCommand]
    private void NavTo(object i)
    {
        //Takes in a command parameter from the xaml button and parses it as a string
        CurrentPage = pages[int.Parse(i as string ?? string.Empty)];

        //If it's 1 it goes to the settingsview, if it's 2 it's the leaderboardview so it triggers the respective method
        switch (int.Parse(i as string ?? string.Empty))
        {
            case 1:
                settingsViewModel.GetSettings.Invoke();
                break;
            case 2:
                LeaderBoardViewModel.GetScore.Invoke();
                break;
        }
    }

    // Moves back to the startview and resets userid to zero
    [RelayCommand]
    private void LogOut()
    {
        UiGlobals.UserID = null;

        MainWindowViewModel.NextNav?.Invoke(3);
    }
}