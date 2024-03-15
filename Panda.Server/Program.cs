using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Panda.Server.Models;

namespace Panda.Server;

public abstract class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
            options.Secure = CookieSecurePolicy.Always;
        });


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapGet("/", () => "Welcome to Panda Server!");

        app.MapPost("/login", async ([FromBody] User USER) =>
            {
                //Does an sql query to check if the user exists and if the password is correct
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql =
                    "SELECT UserID, Username, HashedPassword, Question, HashedAnswer FROM User WHERE Username = @Username";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { USER.Username });

                // If the user does not exist or the password is incorrect, return false
                return dbUser != null && Hash.CompareHash(USER.HashedPassword, dbUser.HashedPassword);
            }).WithName("LoginUser")
            .WithOpenApi();

        app.MapPost("/register", async ([FromBody] User USER) =>
            {
                //Does an sql query to check if the user exists
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                var sql =
                    "SELECT UserID, Username, HashedPassword, Question, HashedAnswer FROM User WHERE Username = @Username";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { USER.Username });

                // If the user exists, return false
                if (dbUser != null) return false;
                // If the user does not exist, hashes the password and answer and inserts the user into the database
                USER.HashedPassword = Hash.GetHash(USER.HashedPassword);
                USER.HashedAnswer = Hash.GetHash(USER.HashedAnswer);
                sql =
                    "INSERT INTO User (Username, HashedPassword, Question, HashedAnswer) VALUES (@Username, @HashedPassword, @Question, @HashedAnswer)";
                await connection.ExecuteAsync(sql, USER);
                return true;
            }).WithName("RegisterUser")
            .WithOpenApi();

        app.MapGet("/is_user_registered/{USERNAME}", async ([FromRoute] string USERNAME) =>
            {
                //Does an sql query to check if the user exists
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                var sql = "SELECT Username FROM User WHERE Username = @Username";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = USERNAME });

                // If the user exists, return true
                return dbUser != null;
            }).WithName("IsUserRegistered")
            .WithOpenApi();


        app.MapPut("/update_password", async ([FromBody] User USER) =>
            {
                //Does an sql query to check if the user exists
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                var sql =
                    "SELECT UserID, Username, HashedPassword, Question, HashedAnswer FROM User WHERE UserID = @UserID";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { USER.UserID });

                // If the user does not exist, return
                if (dbUser == null) return;

                // Hashes the password and updates the user in the database
                dbUser.HashedPassword = Hash.GetHash(USER.HashedPassword);
                sql = "UPDATE User SET HashedPassword = @HashedPassword WHERE UserID = @UserID";
                await connection.ExecuteAsync(sql, dbUser);
            }).WithName("UpdatePassword")
            .WithOpenApi();

        app.MapGet("get_user_id/{USERNAME}", async (string USERNAME) =>
            {
                //Does an sql query to check if the user exists
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql = "SELECT UserID FROM User WHERE Username = @Username";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = USERNAME });

                // If the user exists, returns the user id
                return dbUser != null ? Results.Ok(dbUser.UserID) : Results.NotFound();
            }).WithName("GetUserId")
            .Produces<int>()
            .WithOpenApi();

        app.MapGet("/get_username/{USERID:int}", async ([FromRoute] int USERID) =>
            {
                //Does an sql query to check if the user exists
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql = "SELECT Username FROM User WHERE UserID = @UserID";
                var dbUser = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = USERID });

                // If the user exists, returns the username
                return dbUser != null ? Results.Ok(dbUser.Username) : Results.NotFound();
            }).WithName("GetUsername")
            .Produces<string>()
            .WithOpenApi();

        app.MapGet("/get_auth_questions/{USERID:int}", async (int USERID) =>
            {
                //Does an sql query to get the user's security question and hashed answer
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql = "SELECT Question, HashedAnswer FROM User WHERE UserID = @UserID";
                var authQuestions = await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserID = USERID });

                // If the user exists, returns the security question and hashed answer
                return authQuestions != null ? Results.Ok(authQuestions) : Results.NotFound();
            }).WithName("GetAuthQuestions")
            .Produces<User>()
            .WithOpenApi();

        app.MapGet("/get_settings/{USERID:int}", async ([FromRoute] int USERID) =>
            {
                //Does an sql query to get the user's settings
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql =
                    "SELECT SettingsID,Name, Up, `Left`, Down, `Right`, Reset, Volume, ScreenRatio, Build, UserID FROM Settings WHERE UserID = @USERID";
                var settings = await connection.QueryAsync<Settings>(sql, new { UserID = USERID });

                // returns the settings
                return Results.Ok(settings);
            }).WithName("GetSettings")
            .Produces<IEnumerable<Settings>>()
            .WithOpenApi();

        app.MapGet("/get_saves/{USERID:int}", async ([FromRoute] int USERID) =>
            {
                //Does an sql query to get the user's saves
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql = "SELECT SaveID,Choice, LastPlayed, UserID FROM Saves WHERE UserID = @UserID";
                var saves = await connection.QueryAsync<Save>(sql, new { UserID = USERID });

                // returns the saves
                return Results.Ok(saves);
            }).WithName("GetSaves")
            .Produces<IEnumerable<Save>>()
            .WithOpenApi();

        app.MapGet("/get_scores", async () =>
            {
                //Does an sql query to get the scores
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql = "SELECT ScoreID, HighScore, LongestLived, UserID, `Character` FROM Score";
                var scores = await connection.QueryAsync<Score>(sql);

                // returns the scores
                return Results.Ok(scores);
            }).WithName("GetScores")
            .Produces<IEnumerable<Score>>()
            .WithOpenApi();


        app.MapPost("/save_settings", async ([FromBody] List<Settings> SETTINGSLIST) =>
            {
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));

                // does an sql query to check if the settings exist
                for (var i = SETTINGSLIST.Count - 1; i >= 0; i--)
                {
                    var settings = SETTINGSLIST[i];
                    var sql = "SELECT COUNT(*) FROM Settings WHERE UserID = @UserID AND Name = @Name";
                    var count = await connection.ExecuteScalarAsync<int>(sql, new { settings.UserID, settings.Name });

                    if (count > 0)
                    {
                        // Updates existing settings
                        sql =
                            "UPDATE Settings SET Up = @Up, Down = @Down, `Left` = @Left, `Right` = @Right, Build = @Build WHERE UserID = @UserID AND Name = @Name";
                        await connection.ExecuteAsync(sql, settings);
                    }
                    else
                    {
                        // Inserts new settings
                        sql =
                            "INSERT INTO Settings(UserID, Name, Up, Down, `Left`, `Right`, Build, Reset, Volume, ScreenRatio) VALUES (@UserID, @Name, @Up, @Down, @Left, @Right, @Build, @Reset, @Volume, @ScreenRatio)";
                        await connection.ExecuteAsync(sql, settings);
                    }
                }
            }).WithName("SaveSettings")
            .WithOpenApi();


        app.MapPost("/save_game", async ([FromBody] Save SAVE) =>
        {
            SAVE.LastPlayed = DateTime.Now; // Sets LastPlayed to the current date and time

            //Does an sql query to insert the save into the database and get the save id
            await using var connection =
                new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
            const string sql =
                "INSERT INTO Saves (Choice, LastPlayed, UserID) VALUES (@Choice, @LastPlayed, @UserID); " +
                "SELECT LAST_INSERT_ID();";
            var saveId = await connection.ExecuteScalarAsync<int>(sql, SAVE);

            // returns the save id            
            return saveId;
        }).WithName("SaveGame");


        app.MapPut("update_save", async ([FromBody] Save SAVE) =>
        {
            await using var connection =
                new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
            // Sets LastPlayed to the current date and time
            SAVE.LastPlayed = DateTime.Now;

            // Does an sql query to update the save in the database
            const string sql = "UPDATE Saves SET LastPlayed = @LastPlayed WHERE SaveID = @SaveID";
            await connection.ExecuteAsync(sql, SAVE);
        }).WithName("UpdateSave");

        app.MapPost("/save_score", async ([FromBody] Score SCORE) =>
            {
                // Set the points to 0 at the start
                SCORE.HighScore = 0;
                SCORE.LongestLived = 0;

                // Does an sql query to insert the score into the database and get the score id
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
                const string sql =
                    "INSERT INTO Score (HighScore, LongestLived, UserID, `Character`) VALUES (@HighScore, @LongestLived, @UserID, @Character); " +
                    "SELECT LAST_INSERT_ID();";
                var scoreId = await connection.ExecuteScalarAsync<int>(sql, SCORE);

                // returns the score id
                return scoreId;
            }).WithName("SaveScore")
            .WithOpenApi();


        app.MapPost("update_score", async ([FromBody] Score SCORE) =>
            {
                // Does an sql query to update the score in the database
                await using var connection =
                    new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));

                // Fetch the current HighScore and LongestLived values from the database
                const string fetchSql = "SELECT HighScore, LongestLived FROM Score WHERE ScoreID = @ScoreID";
                var currentScore = await connection.QueryFirstOrDefaultAsync<Score>(fetchSql, new { SCORE.ScoreID });

                if (currentScore != null)
                {
                    // Check if the new HighScore is higher than the current one
                    if (SCORE.HighScore > currentScore.HighScore)
                    {
                        // Update the HighScore in the database
                        const string updateHighScoreSql =
                            "UPDATE Score SET HighScore = @HighScore WHERE ScoreID = @ScoreID";
                        await connection.ExecuteAsync(updateHighScoreSql, new { SCORE.HighScore, SCORE.ScoreID });
                    }

                    // Check if the new LongestLived is higher than the current one
                    if (SCORE.LongestLived > currentScore.LongestLived)
                    {
                        // Update the LongestLived in the database
                        const string updateLongestLivedSql =
                            "UPDATE Score SET LongestLived = @LongestLived WHERE ScoreID = @ScoreID";
                        await connection.ExecuteAsync(updateLongestLivedSql, new { SCORE.LongestLived, SCORE.ScoreID });
                    }
                }
            }).WithName("UpdateScore")
            .WithOpenApi();

        app.Run();
    }
}