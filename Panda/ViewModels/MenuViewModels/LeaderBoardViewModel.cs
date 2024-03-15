using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Panda.Server;
using Panda.Services.DbCTRL;
using Panda.ViewModels.Base;

namespace Panda.ViewModels.MenuViewModels;

public partial class LeaderBoardViewModel : BasePageViewModel
{
    // You know what these are if not, check the mainwindowviewmodel
    [ObservableProperty] private ObservableCollection<Save>? saves;

    [ObservableProperty] private List<Score>? scores;

    [ObservableProperty] private string error;

    // Trigger for leaderboard
    public static Action GetScore { get; private set; }

    public LeaderBoardViewModel()
    {
        // trigger for the getscore method
        GetScore = GetScores;
    }

    public async void GetScores()
    {
        // Gets score from sqlctrl or returns an error
        if (await SqlCTRL.CheckClientConnection())
            Scores = await SqlCTRL.GetScores();
        else
            Error = "No connection to the server";
    }
}