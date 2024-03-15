using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Panda.MGModel;
using Panda.Server;
using Panda.Server.Models;

namespace Panda.Services.DbCTRL;

public static class JsonCTRL
{
    // Gets settings from the settings file
    public static ObservableCollection<Settings>? GetSettings()
    {
        return JsonSerializer.Deserialize<ObservableCollection<Settings>>(File.ReadAllText(FileConsts.SettingsFile));
    }

    public static void SaveSettings(List<Settings> SETTINGS)
    {
        // Gets settings from the settings file
        var jsonsettings = GetSettings();

        // cycles through the settings and updates the json settings
        for (var i = SETTINGS.Count - 1; i >= 0; i--)
        {
            var setting = SETTINGS[i];
            var jsonSetting = jsonsettings?.FirstOrDefault(s => s.Name == setting.Name);
            if (jsonSetting == null) continue;

            for (var j = typeof(Settings).GetProperties().Length - 1; j >= 0; j--)
            {
                var prop = typeof(Settings).GetProperties()[j];
                var newValue = prop.GetValue(setting);
                prop.SetValue(jsonSetting, newValue);
            }
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        File.WriteAllText(FileConsts.SettingsFile, JsonSerializer.Serialize(jsonsettings, options));
    }

    // Gets saves from the save file
    public static ObservableCollection<Save> GetSaves()
    {
        return new ObservableCollection<Save>(JsonSerializer
            .Deserialize<ObservableCollection<Save>>(File.ReadAllText(FileConsts.SaveFile)).Select(s => new Save(s.Choice)
                { LastPlayed = s.LastPlayed, SaveID = s.SaveID, UserID = s.UserID }));
    }
}