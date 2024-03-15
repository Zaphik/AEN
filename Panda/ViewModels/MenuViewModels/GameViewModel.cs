using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels.GameViewModels;

namespace Panda.ViewModels.MenuViewModels;

// Follows the same pattern as mainwindowviewmodel
public partial class GameViewModel : BasePageViewModel
{
    public static Action<int> NextNav { get; private set; }

    [ObservableProperty] private BasePageViewModel currentPage;
    private readonly ObservableCollection<BasePageViewModel> pages;

    public GameViewModel(IServiceProvider SERVICES)
    {
        pages =
        [
            SERVICES.GetRequiredService<PickGameViewModel>(),
            SERVICES.GetRequiredService<LoadGameViewModel>(),
            SERVICES.GetRequiredService<NewGameViewModel>()
        ];
        CurrentPage = pages[0];
        NextNav = i => CurrentPage = pages[i];
    }
}