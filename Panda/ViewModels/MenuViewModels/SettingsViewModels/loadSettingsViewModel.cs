using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.MGModel;
using Panda.Server;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.MenuViewModels.SettingsViewModels;

public partial class loadSettingsViewModel : BasePageViewModel
{
    [ObservableProperty] private ObservableCollection<Settings>? settingsList;


    [ObservableProperty] private Settings? selectedSetting;

    [ObservableProperty] private string error;

    public loadSettingsViewModel()
    {
        settingsViewModel.GetSettings = GetSettings;
    }

    private async void GetSettings()
    {
        SettingsList = await SqlCTRL.GetSettings(UiGlobals.UserID);
        if (SettingsList != null)
        {
            SelectedSetting = SettingsList[0];
            UiGlobals.settingsList = SettingsList;
        }

        if (SelectedSetting != null) GameState.Settings = SelectedSetting;
    }


    [RelayCommand]
    private async Task Confirm()
    {
        if (IsValidSetting())
        {
            Error = string.Empty;
            GameState.Settings = SelectedSetting;
            if (UiGlobals.UserID != null)
                if (SettingsList != null)
                    await SqlCTRL.SaveSettings([..SettingsList]);
            JsonCTRL.SaveSettings([..SettingsList]);
        }
    }

    public bool IsValidSetting()
    {
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
    private void AddSettings()
    {
        settingsViewModel.AddSetting?.Invoke();

        settingsViewModel.NextNav?.Invoke(1);
    }
}