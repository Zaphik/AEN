using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.Server;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.MenuViewModels.SettingsViewModels;

public partial class addSettingsViewModel : BasePageViewModel
{
    [ObservableProperty] private Settings selectedSetting;
    [ObservableProperty] private string error;

    public addSettingsViewModel()
    {
        settingsViewModel.AddSetting = () => SelectedSetting = new Settings { UserID = UiGlobals.UserID };
    }


    // Confirms if the setting is valid
    [RelayCommand]
    private void ConfirmChoice()
    {
        if (!IsValidSetting()) return;
        UiGlobals.settingsList.Add(SelectedSetting);

        settingsViewModel.NextNav?.Invoke(0);
    }

    public bool IsValidSetting()
    {
        if (string.IsNullOrEmpty(SelectedSetting.Name))
        {
            Error = "Name cannot be empty";
            return false;
        }

        if (UiGlobals.settingsList.Any(setting => setting.Name == SelectedSetting.Name))
        {
            Error = "A setting with this name already exists";
            return false;
        }

        var keys = new List<string>
        {
            SelectedSetting.Up, SelectedSetting.Down, SelectedSetting.Left, SelectedSetting.Right,
            SelectedSetting.Build, SelectedSetting.Reset
        };

        // Check if all keys are characters and unique
        var charKeys = new HashSet<char>();
        for (var i = keys.Count - 1; i >= 0; i--)
        {
            var key = keys[i];
            if (string.IsNullOrEmpty(key))
            {
                Error = "Key cannot be empty";
                return false;
            }

            if (key.Length != 1)
            {
                Error = "Key must be a single character";
                return false;
            }

            if (char.TryParse(key, out var charKey) && charKeys.Add(charKey)) continue;
            Error = "Keys must be unique";
            return false;
        }

        // If all keys are characters and unique, return true
        Error = string.Empty;
        return true;
    }

    [RelayCommand]
    public void Back()
    {
        settingsViewModel.NextNav?.Invoke(0);
    }
}