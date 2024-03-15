using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Panda.ViewModels.Base;

namespace Panda.ViewModels;

//Container for each view
public partial class MainWindowViewModel : BasePageViewModel
{
    // Delegate for navigation
    public static Action<int> NextNav { get; private set; }

    // Reflects the viewmodel to its corresponding view on the screen and allows for binding
    [ObservableProperty] private BasePageViewModel currentPage;

    // Observable collection of all the pages over list to allow for binding and change tracking
    private ObservableCollection<BasePageViewModel> pages;


    public MainWindowViewModel(IServiceProvider SERVICES)
    {
        // Gets singletons from dependency injection and uses them to create a list of Viewmodels
        pages =
        [
            SERVICES.GetRequiredService<RegisterViewModel>(),
            SERVICES.GetRequiredService<LoginViewModel>(),
            SERVICES.GetRequiredService<MenuViewModel>(),
            SERVICES.GetRequiredService<StartViewModel>()
        ];


        // Sets the current page to the start page
        CurrentPage = pages[3];

        // Sets the next page to the value given by the navigation delegate
        NextNav = i => CurrentPage = pages[i];
    }
}