using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels.GameViewModels;

namespace Panda.ViewModels.LoginViewModels;

public partial class resetPasswordViewModel : BasePageViewModel
{
    private static int UserID { get; set; }

    //you already know what this is
    [ObservableProperty] private string password, rePassword, error;

    public resetPasswordViewModel()
    {
        //values received from forgotpasswordviewmodel
        forgotPasswordViewModel.PassUserId = userid => UserID = userid!;
    }

    [RelayCommand]
    private async Task ResetBtn()
    {
        //Checks if both fields are empty and returns
        if (string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(RePassword))
        {
            Error = "Ya gotta fill in the blanks";
            return;
        }

        //Checks if password is empty and returns
        if (string.IsNullOrEmpty(Password))
        {
            Error = "Ya gotta enter the password as well!!!";
            return;
        }

        //Checks if rePassword is empty and returns
        if (string.IsNullOrEmpty(RePassword))
        {
            Error = "Ya gotta re-enter the password!!!";
            return;
        }


        // Checks if the passwords don't match and returns
        if (Password != RePassword)
        {
            Error = "Passwords do not match";
            return;
        }


        // checks if the password is too long and returns
        switch (Password.Length)
        {
            case > 20:
                Error = "Ya filled in too much ya donut!!!";
                break;
            default:
                //Updates the password and navigates to menuview
                await SqlCTRL.UpdatePassword(UserID, Password);
                UiGlobals.UserID = UserID;
                //triggers the get save method
                LoadGameViewModel.GetSave();
                // Navigates to menuview and resets the loginviews
                MainWindowViewModel.NextNav?.Invoke(2);

                LoginViewModel.NextNav?.Invoke(0);

                Reset();
                break;
        }
    }

    public void Reset()
    {
        Password = string.Empty;
        RePassword = string.Empty;
        Error = string.Empty;
    }
}