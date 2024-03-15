using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels.SettingsViewModels;

namespace Panda.ViewModels.MenuViewModels;

// Follows the same pattern as mainwindowviewmodel
public partial class settingsViewModel : BasePageViewModel
{
    public static Action<int> NextNav { get; private set; }

    // Trigger for getting the settings
    public static Action GetSettings { get; set; }

    // Trigger for adding a setting
    public static Action AddSetting { get; set; }

    [ObservableProperty] private BasePageViewModel currentPage;
    private readonly ObservableCollection<BasePageViewModel> pages;

    public settingsViewModel(IServiceProvider SERVICES)
    {
        pages =
        [
            SERVICES.GetRequiredService<loadSettingsViewModel>(),
            SERVICES.GetRequiredService<addSettingsViewModel>()
        ];
        CurrentPage = pages[0];
        NextNav = i => CurrentPage = pages[(int)i!];
    }
}