using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;
using Panda.ViewModels.RegisterViewModels;

namespace Panda.ViewModels;

//Container for each register view, follows the same pattern as MainWindowViewModel
public partial class RegisterViewModel : BasePageViewModel
{
    public static Action<int> NextNav { get; private set; }

    [ObservableProperty] private BasePageViewModel currentPage;
    private readonly ObservableCollection<BasePageViewModel> pages;

    private IServiceProvider Services;

    public RegisterViewModel(IServiceProvider SERVICES)
    {
        Services = SERVICES;
        pages =
        [
            Services.GetRequiredService<regViewModel>(),
            Services.GetRequiredService<regQuestionViewModel>()
        ];

        CurrentPage = pages[0];
        NextNav = i => CurrentPage = pages[i];
    }


    //  resets the registerViewModel and switches to the loginViewModel
    // Roslyn will implement ICommand for the method and allow for binding
    [RelayCommand]
    private void navLgn()
    {
        //  resets the registerViewModel
        Services.GetRequiredService<regQuestionViewModel>().Reset();
        Services.GetRequiredService<regViewModel>().Reset();
        CurrentPage = pages[0];

        //switches to loginViewModel
        MainWindowViewModel.NextNav?.Invoke(1);
    }
}