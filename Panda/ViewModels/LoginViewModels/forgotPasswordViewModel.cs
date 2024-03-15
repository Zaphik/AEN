using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Server.Models;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.LoginViewModels;

public partial class forgotPasswordViewModel : BasePageViewModel
{
    // all these strings require inotifypropertychanged for reactivity
    [ObservableProperty] private static string question, answer, error;

    //Values which will be received from lgnviewmodel
    private static string Username { get; set; }
    private static string Qquestion { get; set; }
    private static string AAnswer { get; set; }

    //Passes the userid to resetpasswordviewmodel
    public static Action<int> PassUserId { get; set; }

    private int UserID { get; set; }

    public forgotPasswordViewModel()
    {
        //values received from lgnviewmodel
        lgnViewModel.PassUsername = USERNAME => Username = USERNAME;
        lgnViewModel.PassQues = (question1, answer1) =>
        {
            Qquestion = question1;
            AAnswer = answer1;
        };
        Question = Qquestion;
    }

    [RelayCommand]
    private async Task NextBtn()
    {
        //Checks if anything is empty and returns
        if (string.IsNullOrEmpty(Answer))
        {
            Error = "Ya gotta fill in everything!!!";
            return;
        }

        //Compares the hashing
        if (Hash.CompareHash(Answer.ToLower(), AAnswer))
        {
            //Gets userid globally and passes it on to resetpasswordview whilst navigating there
            UserID = await SqlCTRL.GetUserId(Username);
            PassUserId?.Invoke(UserID);
            LoginViewModel.NextNav?.Invoke(2);

            //resets this view
            Reset();
        }
        else
        {
            //Displays error if the answer is wrong
            Error = "Wrong Answer";
        }
    }

    public void Reset()
    {
        Question = string.Empty;
        Answer = string.Empty;
        Error = string.Empty;
    }
}