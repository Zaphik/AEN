using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.RegisterViewModels;

public partial class regViewModel : BasePageViewModel
{
    // all these strings require inotifypropertychanged for reactivity
    [ObservableProperty] private string error, passChar, regpassword, regrepassword, regusername;

    // integer for toggling the password character
    private int intCharPass;

    // Passes the username and password to the regQuestionViewModel
    public static Action<List<string>>? PassReg { get; set; }

    public regViewModel()
    {
        intCharPass = 1;
        PassChar = "*";
    }

    // toggles the password between hidden and shown  
    [RelayCommand]
    private void CharPass()
    {
        intCharPass++;
        PassChar = (intCharPass % 2) switch
        {
            0 => "",
            _ => "*"
        };
    }


    [RelayCommand]
    private async void regBtn()
    {
        // If there isn't any registering error
        if (await SqlCTRL.NoRegError(Regusername, Regpassword, Regrepassword))
        {
            // Passes info and switches to regQuestionViewModel
            PassReg?.Invoke([Regusername, Regpassword]);
            RegisterViewModel.NextNav?.Invoke(1);

            // Resets the viewmodel
            Reset();
        }
        else
        {
            //Gets the respective error from sqlctrl
            Error = SqlCTRL.Error;
        }
    }


    public void Reset()
    {
        // empties everything and resets the password character values
        Regusername = string.Empty;
        Regpassword = string.Empty;
        Regrepassword = string.Empty;
        Error = string.Empty;
        PassChar = "*";
        intCharPass = 1;
    }
}