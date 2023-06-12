using dotenv.net;
using TODO.Models;

namespace TODO.BusinessLayer;

public abstract class Main
{
    public static bool AddUser(Signup signup)
    {
        if (DataAccessLayer.Main.FindUser(signup.username, signup.email)) return false;
        try
        {
            var token = BCrypt.Net.BCrypt.HashPassword(signup.password);
            signup.personalToken = token;
            var result = DataAccessLayer.Main.InsertUser(signup);
            return result;
        }
        catch (Exception e) { throw new Exception(e.Message); }
    }

    public static string? EnterUser(Login login)
    {
        try
        {
            DotEnv.Load();
            var jwt = DataAccessLayer.Main.LoginUser(login);
            return jwt != "" ? jwt : "";
        }
        catch (Exception e) { throw new Exception(e.Message); }
    }

    public static List<object> GetAllUsers()
    {
        List<object> response = new List<object>();
        using var db = new Database();
        foreach (var signup in db.Signups)
        {
            response.Add(new
            {
                id = signup.Id,
                personalToken = signup.personalToken,
                firstName = signup.firstName,
                lastName = signup.lastName,
                username = signup.username,
                email = signup.email,
                password = signup.password
            });
        }

        return response;
    }

    public static bool AddIssue(Issue issue)
    {
        try
        {
            var result = DataAccessLayer.Main.InsertIssue(issue);
            return result;
        }
        catch (Exception e) { throw new Exception(e.Message); }
    }

    public static List<object> GetAllIssue()
    {
        List<object> response = new List<object>();
        using var db = new Database();
        foreach (var issue in db.Issues)
        {
            response.Add(new
            {
                id = issue.Id,
                summary = issue.Summary,
                reporter = issue.Reporter,
                description = issue.Description,
                priority = issue.Priority,
                condition = issue.Condition
            });
        }

        return response;
    }

    public static bool EditIssue(Issue newIssue)
    {
        try
        {
            var result = DataAccessLayer.Main.EditIssue(newIssue);
            return result;
        }
        catch (Exception e) { throw new Exception(e.Message); }
    }

    public static bool DeleteIssue(int id)
    {
        try
        {
            var result = DataAccessLayer.Main.DeleteIssue(id);
            return result;
        }
        catch (Exception e) { throw new Exception(e.Message); }
    }
}