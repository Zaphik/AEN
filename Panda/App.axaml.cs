using System.Collections.ObjectModel;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Panda.MGModel;
using Panda.Server;
using Panda.Services.DbCTRL;
using Panda.ViewModels;
using Panda.ViewModels.LoginViewModels;
using Panda.ViewModels.MenuViewModels;
using Panda.ViewModels.MenuViewModels.GameViewModels;
using Panda.ViewModels.MenuViewModels.SettingsViewModels;
using Panda.ViewModels.RegisterViewModels;
using Panda.Views;
using Panda.Views.LoginViews;
using Panda.Views.MenuViews;
using Panda.Views.MenuViews.GameViews;
using Panda.Views.MenuViews.SettingsViews;
using Panda.Views.RegisterViews;
using Path = System.IO.Path;

namespace Panda;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        GameState.Settings ??= JsonCTRL.GetSettings()?[0];


        RegisterServices(out var services);


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = services.BuildServiceProvider().GetRequiredService<MainWindow>();


        base.OnFrameworkInitializationCompleted();
    }

    //All these methods are used to register the services and viewmodels to the application and then set the views datacontext to the viewmodels
    // It also sets passes in the ServiceProvider to all viewmodels that require it to ensure that they can access the exact same instance of the service as the rest of the application
    private void RegisterServices(out ServiceCollection services)
    {
        services = [];
        LoginServices(services);
        RegServices(services);
        MenuServices(services);
        services.AddSingleton<MainWindow>(s => new MainWindow
        {
            DataContext = s.GetRequiredService<MainWindowViewModel>()
        });
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<StartViewModel>();
        services.AddSingleton<StartView>(s => new StartView
        {
            DataContext = s.GetRequiredService<StartViewModel>()
        });

        services.BuildServiceProvider();
    }

    private void LoginServices(IServiceCollection services)
    {
        services.AddSingleton<lgnViewModel>();
        services.AddSingleton<lgnView>(s => new lgnView
        {
            DataContext = s.GetRequiredService<lgnViewModel>()
        });
        services.AddSingleton<resetPasswordViewModel>();
        services.AddSingleton<resetPasswordView>(s => new resetPasswordView
        {
            DataContext = s.GetRequiredService<resetPasswordViewModel>()
        });
        services.AddTransient<forgotPasswordViewModel>();
        services.AddSingleton<forgotPasswordView>(s => new forgotPasswordView
        {
            DataContext = s.GetRequiredService<forgotPasswordViewModel>()
        });
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<LoginView>(s => new LoginView
        {
            DataContext = s.GetRequiredService<LoginViewModel>()
        });
    }

    private void RegServices(IServiceCollection services)
    {
        services.AddSingleton<regViewModel>();
        services.AddSingleton<regView>(s => new regView
        {
            DataContext = s.GetRequiredService<regViewModel>()
        });
        services.AddSingleton<regQuestionViewModel>();
        services.AddSingleton<regQuestionView>(s => new regQuestionView
        {
            DataContext = s.GetRequiredService<regQuestionViewModel>()
        });
        services.AddSingleton<RegisterViewModel>();
        services.AddSingleton<RegisterView>(s => new RegisterView
        {
            DataContext = s.GetRequiredService<RegisterViewModel>()
        });
    }

    private void MenuServices(IServiceCollection services)
    {
        services.AddSingleton<LeaderBoardViewModel>();
        services.AddSingleton<LeaderBoardView>(s => new LeaderBoardView
        {
            DataContext = s.GetRequiredService<LeaderBoardViewModel>()
        });
        services.AddSingleton<MenuView>(s => new MenuView
        {
            DataContext = s.GetRequiredService<MenuViewModel>()
        });
        services.AddSingleton<MenuViewModel>();
        SettingsServices(services);
        GameServices(services);
    }

    private void SettingsServices(IServiceCollection services)
    {
        services.AddSingleton<settingsViewModel>();
        services.AddSingleton<settingsView>(s => new settingsView
        {
            DataContext = s.GetRequiredService<settingsViewModel>()
        });
        services.AddSingleton<loadSettingsViewModel>();
        services.AddSingleton<loadSettingsView>(s => new loadSettingsView
        {
            DataContext = s.GetRequiredService<loadSettingsViewModel>()
        });
        services.AddSingleton<addSettingsViewModel>();
        services.AddSingleton<addSettingsView>(s => new addSettingsView
        {
            DataContext = s.GetRequiredService<addSettingsViewModel>()
        });
        services.AddSingleton<ObservableCollection<Settings>>();
    }

    private void GameServices(IServiceCollection services)
    {
        services.AddSingleton<GameViewModel>();
        services.AddSingleton<GameView>(s => new GameView
        {
            DataContext = s.GetRequiredService<GameViewModel>()
        });
        services.AddSingleton<NewGameViewModel>();
        services.AddSingleton<NewGameView>(s => new NewGameView
        {
            DataContext = s.GetRequiredService<NewGameViewModel>()
        });
        services.AddSingleton<LoadGameViewModel>();
        services.AddSingleton<LoadGameView>(s => new LoadGameView
        {
            DataContext = s.GetRequiredService<LoadGameViewModel>()
        });
        services.AddSingleton<PickGameViewModel>();
        services.AddSingleton<PickGameView>(s => new PickGameView
        {
            DataContext = s.GetRequiredService<PickGameViewModel>()
        });
    }
}