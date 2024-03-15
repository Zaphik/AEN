using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;
using Panda.ViewModels.MenuViewModels.GameViewModels;

namespace Panda.ViewModels.RegisterViewModels;

public partial class regQuestionViewModel : BasePageViewModel
{
    //Values which will be received from regviewmodel
    private static string Username { get; set; }
    private static string Password { get; set; }

    // all these strings require inotifypropertychanged for reactivity
    [ObservableProperty] private string answer, error, question;


    public regQuestionViewModel()
    {
        //values received from regviewmodel
        regViewModel.PassReg = INFO =>
        {
            var info = (List<string>)INFO!;
            Username = info[0];
            Password = info[1];
        };
    }


    [RelayCommand]
    private async Task SendBtn()
    {
        //Checks if anything is empty and returns
        if (string.IsNullOrEmpty(Answer) || string.IsNullOrEmpty(Question))
        {
            Error = "Ya gotta fill everything in!!";
            return;
        }

        // Else tries to register user
        if (await SqlCTRL.RegisterUser(Username, Password, Question, Answer.ToLower()))
        {
            //Gets the userid and sets it globally
            UiGlobals.UserID = await SqlCTRL.GetUserId(Username);

            //trigger the get save method
            LoadGameViewModel.GetSave();

            // Navigates to menuview and resets the registerviews
            MainWindowViewModel.NextNav?.Invoke(2);
            RegisterViewModel.NextNav?.Invoke(0);
            Reset();
        }
    }

    //Empties everything
    public void Reset()
    {
        Question = string.Empty;
        Answer = string.Empty;
        Error = string.Empty;
    }
}