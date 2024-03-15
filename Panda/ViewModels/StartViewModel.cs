using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels.GameViewModels;

namespace Panda.ViewModels;

public partial class StartViewModel : BasePageViewModel
{
    // Error that can be binded to and changed in the view
    [ObservableProperty] private string error;
    
    //Task that checks if the user is connected to the server

    [RelayCommand]
    private async Task Online()
    {
        if (await SqlCTRL.CheckClientConnection())
            MainWindowViewModel.NextNav?.Invoke(0);
        else
            // Sets the Error if not
            Error = "No Connection";
    }


    [RelayCommand]
    private void Offline()
    {
        // trigger the getsaves method
        LoadGameViewModel.GetSave();

        //Navigates to the menuviewmodel
        MainWindowViewModel.NextNav?.Invoke(2);
    }

    //Exits ot of the app

    [RelayCommand]
    private void Exit()
    {
        System.Environment.Exit(0);
    }
}