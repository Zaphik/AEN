using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;
using Panda.ViewModels.LoginViewModels;

namespace Panda.ViewModels;

// Container for login views, follows the exact same pattern RegisterViewModel
public partial class LoginViewModel : BasePageViewModel
{
    public static Action<int> NextNav { get; private set; }

    [ObservableProperty] private BasePageViewModel currentPage;
    private readonly ObservableCollection<BasePageViewModel> pages;

    private IServiceProvider Services;

    public LoginViewModel(IServiceProvider SERVICES)
    {
        Services = SERVICES;
        pages =
        [
            Services.GetRequiredService<lgnViewModel>(),
            Services.GetRequiredService<forgotPasswordViewModel>(),
            Services.GetRequiredService<resetPasswordViewModel>()
        ];
        CurrentPage = pages[0];
        NextNav = i => CurrentPage = pages[i];
    }

    [RelayCommand]
    private void navReg()
    {
        //  resets the loginViewModel
        Services.GetRequiredService<resetPasswordViewModel>().Reset();
        Services.GetRequiredService<forgotPasswordViewModel>().Reset();
        Services.GetRequiredService<lgnViewModel>().Reset();
        CurrentPage = pages[0];

        //switches to registerViewModel
        MainWindowViewModel.NextNav?.Invoke(0);
    }
}