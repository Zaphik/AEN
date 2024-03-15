using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Panda.Server;
using static System.String;

namespace Panda.Services.DbCTRL;

// Alot of this will be explained in the panda.server.program
public abstract class SqlCTRL
{
    // Client that allows connection to the server
    private static readonly HttpClient Client;

    // string that holds the error message
    public static string Error { get; private set; }

    // string that holds the forgot password message
    public static string ForgotPassword { get; private set; }


    //List for score
    private static List<Score>? scores { get; set; }

    static SqlCTRL()
    {
        Client = new HttpClient();
    }

    //Checks client connection
    public static async Task<bool> CheckClientConnection()
    {
        try
        {
            var response = await Client.GetAsync("https://localhost:7261");
            return response.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }


    public static async Task<bool> LoginUser(string USERNAME, string PASSWORD)
    {
        //Checks if the username and password are empty
        if (IsNullOrEmpty(USERNAME) && IsNullOrEmpty(PASSWORD))
        {
            Error = "Ya got to fill in everything ya donut!!!";
            return false;
        }

        //Checks if the username is empty
        if (IsNullOrEmpty(USERNAME))
        {
            Error = "Ya gotta fill in yer username ya donut!!!";
            return false;
        }

        //Checks if the password is empty
        if (IsNullOrEmpty(PASSWORD))
        {
            Error = "Ya gotta fill in yer password ya donut!!!";
            return false;
        }

        //Checks if the username and password are too long
        if (USERNAME.Length >= 16 || PASSWORD.Length >= 20)
        {
            Error = "Ya filled in too much ya donut!!!";
            return false;
        }

        //Checks if the user is already registered
        if (await AlreadyRegisteredUser(USERNAME))
        {
            var user = new User
            {
                Username = USERNAME,
                HashedPassword = PASSWORD
            };
            var json = JsonSerializer.Serialize(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("https://localhost:7261/login", data);

            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<bool>();
            if (result) return true;

            // if false, sets the error message and forgotpassword message
            Error = "Ya got the wrong password ya donut!!!";
            ForgotPassword = "Forgot Password?";
            return false;
        }

        // if false, sets the error message
        Error = "This account don't exist";
        return false;
    }

    //sends a post request and returns the results
    public static async Task<bool> RegisterUser(string USERNAME, string PASSWORD, string QUESTION, string ANSWER)
    {
        var user = new User
        {
            Username = USERNAME,
            HashedPassword = PASSWORD,
            Question = QUESTION,
            HashedAnswer = ANSWER
        };
        var json = JsonSerializer.Serialize(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("https://localhost:7261/register", data);

        if (!response.IsSuccessStatusCode) return false;
        var result = await response.Content.ReadFromJsonAsync<bool>();
        return result;
    }

    //sends a put request and returns the results
    public static async Task UpdatePassword(int USERID, string PASSWORD)
    {
        var user = new User
        {
            UserID = USERID,
            HashedPassword = PASSWORD
        };

        var json = JsonSerializer.Serialize(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync("https://localhost:7261/update_password", data);

        if (!response.IsSuccessStatusCode) throw new Exception($"Failed to update password: {response.StatusCode}");
    }

    //sends a get request and returns the results
    public static async Task<int> GetUserId(string USERNAME)
    {
        var response = await Client.GetAsync($"https://localhost:7261/get_user_id/{USERNAME}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get user id: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return await response.Content.ReadFromJsonAsync<int>();
    }

    //sends a get request and returns the results
    private static async Task<string> GetUsername(int USERID)
    {
        var response = await Client.GetAsync($"https://localhost:7261/get_username/{USERID}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get username: {response.StatusCode}, {response.ReasonPhrase}");
        }

        return await response.Content.ReadFromJsonAsync<string>();
    }

    //sends a get request and returns the results
    public static async Task<string[]> GetAuthQuestions(int USERID)
    {
        var response = await Client.GetAsync($"https://localhost:7261/get_auth_questions/{USERID}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get auth questions: {response.StatusCode}, {response.ReasonPhrase}");
        }

        var user = await response.Content.ReadFromJsonAsync<User>();

        return [user?.Question, user?.HashedAnswer];
    }


    public static async Task<ObservableCollection<Save>?> GetSaves(int? USERID)
    {
        //sends a get request
        ObservableCollection<Save>? saves;
        try
        {
            var saveData =
                await Client.GetFromJsonAsync<ObservableCollection<Save>>($"https://localhost:7261/get_saves/{USERID}");

            saves = new ObservableCollection<Save>(saveData.Select(s => new Save(s.Choice)
                { LastPlayed = s.LastPlayed, SaveID = s.SaveID, UserID = s.UserID }));

            var collection = JsonCTRL.GetSaves();
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                var save = collection[i];
                saves.Add(save);
            }

            //returns the results and adds on the results from the jsonctrl
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //if the request fails, returns the results from the jsonctrl
            saves = JsonCTRL.GetSaves();
        }

        return saves;
    }


    public static async Task<ObservableCollection<Settings>?> GetSettings(int? USERID)
    {
        //sends a get request
        ObservableCollection<Settings>? settings;
        try
        {
            settings = await Client.GetFromJsonAsync<ObservableCollection<Settings>>(
                $"https://localhost:7261/get_settings/{USERID}");
            for (var i = JsonCTRL.GetSettings()!.Count - 1; i >= 0; i--)
                settings?.Insert(0, JsonCTRL.GetSettings()?[i]!);
            //returns the results and adds on the results from the jsonctrl
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //if the request fails, returns the results from the jsonctrl
            settings = JsonCTRL.GetSettings();
        }

        return settings;
    }

    //sends a get request and returns the results
    public static async Task<List<Score>?> GetScores()
    {
        scores = await Client.GetFromJsonAsync<List<Score>>("https://localhost:7261/get_scores");

        if (scores == null) return scores;
        for (var i = scores.Count - 1; i >= 0; i--)
        {
            var score = scores[i];
            if (score.UserID != null) score.Username = await GetUsername(score.UserID.Value);
        }

        return scores;
    }


    public static async Task SaveSettings(IEnumerable<Settings> SETTINGSLIST)
    {
        // gets the settings from the jsonctrl
        var jsonSettings = JsonCTRL.GetSettings();

        // filters the settings by taking the settings that are not in the json settings
        var filteredSettings = SETTINGSLIST.Where(s =>
            jsonSettings != null && !jsonSettings.Any(js => js.Name == s.Name && js.UserID == s.UserID)).ToList();

        // sends a post request
        var json = JsonSerializer.Serialize(filteredSettings);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        // if request fails, throws an exception
        var response = await Client.PostAsync("https://localhost:7261/save_settings", data);

        if (!response.IsSuccessStatusCode) throw new Exception($"Failed to save settings: {response.StatusCode}");
    }


    // sends a put request and throws an exception if the request fails
    public static async Task UpdateSave(int SAVEID)
    {
        var save = new Save(null)
        {
            SaveID = SAVEID,
            LastPlayed = DateTime.Now
        };

        var json = JsonSerializer.Serialize(save);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PutAsync("https://localhost:7261/update_save", data);

        if (!response.IsSuccessStatusCode) throw new Exception($"Failed to update save: {response.StatusCode}");
    }

    // sends a post request and throws an exception if the request fails
    public static async Task UpdateScore(int SCOREID, int POINTS, int TIMEPLAYED, string CHARACTER)
    {
        var score = new Score
        {
            ScoreID = SCOREID,
            HighScore = POINTS,
            Character = CHARACTER,
            LongestLived = TIMEPLAYED
        };
        var json = JsonSerializer.Serialize(score);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("https://localhost:7261/update_score", data);

        if (!response.IsSuccessStatusCode) throw new Exception($"Failed to update score: {response.StatusCode}");
    }

    // sends a post request and returns the results
    public static async Task<int?>? SaveGame(string? CHOICE, int? USERID)
    {
        var save = new Save(null)
        {
            Choice = CHOICE,
            UserID = USERID
        };
        try
        {
            var json = JsonSerializer.Serialize(save);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("https://localhost:7261/save_game", data);

            var saveId = await response.Content.ReadFromJsonAsync<int>();
            return saveId;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // sends a post request and returns the results
    public static async Task<int> SaveScore(int USERID, string CHARACTER)
    {
        var score = new Score
        {
            UserID = USERID,
            Character = CHARACTER
        };
        var json = JsonSerializer.Serialize(score);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PostAsync("https://localhost:7261/save_score", data);

        if (!response.IsSuccessStatusCode) throw new Exception($"Failed to save score: {response.StatusCode}");

        var scoreId = await response.Content.ReadFromJsonAsync<int>();
        return scoreId;
    }

    //sends a get request and returns the results
    public static async Task<bool> AlreadyRegisteredUser(string USERNAME)
    {
        var response = await Client.GetAsync($"https://localhost:7261/is_user_registered/{USERNAME}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to verify credentials: {response.StatusCode}");
        }
        
        var userExists = await response.Content.ReadFromJsonAsync<bool>();
        return userExists;
    }

    public static async Task<bool> NoRegError(string USERNAME, string PASSWORD, string REPASSWORD)
    {
        //Checks if the username, repassword and password are empty
        if (IsNullOrEmpty(USERNAME) && IsNullOrEmpty(REPASSWORD) && IsNullOrEmpty(PASSWORD))
        {
            Error = "Ya gotta fill in everything ya donut!!!";
            return false;
        }

        //Checks if the username and password are empty
        if (IsNullOrEmpty(USERNAME) && IsNullOrEmpty(PASSWORD))
        {
            Error = "Ya gotta fill in yer username and password!!!";
            return false;
        }

        //Checks if the username is empty
        if (IsNullOrEmpty(USERNAME))
        {
            Error = "Ya gotta fill in yer username ya donut!!!";
            return false;
        }

        //Checks if the password is empty
        if (IsNullOrEmpty(PASSWORD))
        {
            Error = "Ya gotta fill in yer password ya donut!!!";
            return false;
        }

        //Checks if the repassword is empty
        if (IsNullOrEmpty(REPASSWORD))
        {
            Error = "Ya gotta re-enter yer password ya donut!!!";
            return false;
        }

        //Checks if the username, repassword and password contain spaces
        if (USERNAME.Contains(' ') || PASSWORD.Contains(' ') || REPASSWORD.Contains(' '))
        {
            Error = "No spaces allowed";
            return false;
        }

        //Checks if the username and password are too long
        if (USERNAME.Length >= 16 || PASSWORD.Length >= 20)
        {
            Error = "Ya filled in too much ya donut!!!";
            return false;
        }

        //Checks if the password and repassword are the same
        if (PASSWORD != REPASSWORD)
        {
            Error = "Make sure yer passwords are the same";
            return false;
        }

        //Checks if the user is already registered
        if (!await AlreadyRegisteredUser(USERNAME)) return true;
        Error = "Ya ain't original enough";
        return false;
    }
}