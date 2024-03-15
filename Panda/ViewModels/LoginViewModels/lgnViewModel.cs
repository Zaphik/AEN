using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.LoginViewModels;

public partial class lgnViewModel : BasePageViewModel
{
    // all these strings require inotifypropertychanged for reactivity
    [ObservableProperty] private string passChar, password, username, forgotPassword, error;

    // integer for toggling the password character
    private int intCharPass { get; set; }

    // Passes Questions and Answers to the forgotpasswordviewmodel
    public static QnA? PassQues { get; set; }

    // Passes the username to the forgotpasswordviewmodel
    public static Action<string> PassUsername { get; set; }

    public lgnViewModel()
    {
        intCharPass = 1;
        passChar = "*";
    }

    // toggles the password between hidden and shown    
    [RelayCommand]
    private void CharPass()
    {
        intCharPass++;
        PassChar = intCharPass % 2 == 0 ? "" : "*";
    }


    [RelayCommand]
    private async Task lgnBtn()
    {
        // checks if the username and password exists in the database
        if (await SqlCTRL.LoginUser(Username, Password))
        {
            // Sets the userid globally and navigates to menuview whilst resetting the loginview
            UiGlobals.UserID = await SqlCTRL.GetUserId(Username);

            MainWindowViewModel.NextNav?.Invoke(2);

            Reset();
        }
        else
        {
            //Gets the respective error from sqlctrl and could show an error for forgotpassword
            ForgotPassword = SqlCTRL.ForgotPassword;
            Error = SqlCTRL.Error;
        }
    }

    [RelayCommand]
    private async void forgotPasswordBtn()
    {
        // Returns if the user isn't registered or there is nothing in password
        if (!await SqlCTRL.AlreadyRegisteredUser(Username) || string.IsNullOrEmpty(Password)) return;

        //gets the userid and questions and passes it to forgotpasswordviewmodel and navigates to there
        //the reason userid is not set globally is because there isn't any confirmation to prove it's the correct user
        var userId = await SqlCTRL.GetUserId(Username);
        var QandA = await SqlCTRL.GetAuthQuestions(userId);
        PassUsername?.Invoke(Username);
        PassQues?.Invoke(QandA[0], QandA[1]);
        LoginViewModel.NextNav?.Invoke(1);

        //resets this view
        Reset();
    }

    public void Reset()
    {
        Username = string.Empty;
        Password = string.Empty;
        Error = string.Empty;
        ForgotPassword = string.Empty;
        PassChar = "*";
        intCharPass = 1;
    }
}