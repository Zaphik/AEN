using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Panda.MGModel;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;
using Save = Panda.Server.Save;

namespace Panda.ViewModels.MenuViewModels.GameViewModels;

public partial class LoadGameViewModel : BasePageViewModel
{
    // yup, you know what these are, if somehow you don't, check the mainwindowviewmodel
    [ObservableProperty] private ObservableCollection<Save>? saves;

    [ObservableProperty] private Save? selectedSave;

    // Trigger for the getsaves method
    public static Action GetSave;


    public LoadGameViewModel()
    {
        // trigger for the getsaves method
        GetSave = GetSaves;
    }


    private async void GetSaves()
    {
        // Gets saves from sqlctrl
        Saves = await SqlCTRL.GetSaves(UiGlobals.UserID);
        if (Saves == null || Saves.Count == 0) return;

        // Sorts saves by last played and sets the selected save to the first save and sets the global save to the selected save
        Saves = MergeSort(Saves);
        SelectedSave = Saves[0];
        GameState.Save = SelectedSave;
    }


    // Command to start the game
    [RelayCommand]
    private void StartGame()
    {
        GameState.Save = SelectedSave;
        using var game = new Pandemonium();
        game.Run();
    }


    // Command to navigate back
    [RelayCommand]
    private void Back()
    {
        GameViewModel.NextNav?.Invoke(0);
    }

    // the sorting algorithm
    private static ObservableCollection<Save> MergeSort(ObservableCollection<Save> SAVES)
    {
        if (SAVES.Count <= 1) return SAVES;

        var left = new ObservableCollection<Save>();
        var right = new ObservableCollection<Save>();

        var middle = SAVES.Count / 2;
        for (var i = middle - 1; i >= 0; i--) left.Add(SAVES[i]);
        for (var i = SAVES.Count - 1; i >= middle; i--) right.Add(SAVES[i]);

        left = MergeSort(left);
        right = MergeSort(right);
        return Merge(left, right);
    }

    private static ObservableCollection<Save> Merge(IList<Save> LEFT, IList<Save> RIGHT)
    {
        var result = new ObservableCollection<Save>();

        while (LEFT.Count > 0 && RIGHT.Count > 0)
            if (LEFT[0].LastPlayed > RIGHT[0].LastPlayed)
            {
                result.Add(LEFT[0]);
                LEFT.RemoveAt(0);
            }
            else
            {
                result.Add(RIGHT[0]);
                RIGHT.RemoveAt(0);
            }

        while (LEFT.Count > 0)
        {
            result.Add(LEFT[0]);
            LEFT.RemoveAt(0);
        }

        while (RIGHT.Count > 0)
        {
            result.Add(RIGHT[0]);
            RIGHT.RemoveAt(0);
        }

        return result;
    }
}