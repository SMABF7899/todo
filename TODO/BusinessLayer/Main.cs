using dotenv.net;
using TODO.Models;
using TODO.PresentationLayer;

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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? EnterUser(Login login)
    {
        try
        {
            var jwt = DataAccessLayer.Main.LoginUser(login);
            return jwt != "" ? jwt : "";
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool AddIssue(Issue issue)
    {
        try
        {
            if (!DataAccessLayer.Main.FindUser(issue.Reporter)) return false;
            var result = DataAccessLayer.Main.InsertIssue(issue);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static List<object> GetAllIssue(string reporter)
    {
        return DataAccessLayer.Main.AllIssuesObjects(reporter);
    }

    public static List<object> GetIssueByFilter(Filter filter)
    {
        var response = new List<object>();
        if (filter.Time == "" && filter.Condition == Condition.None && filter.Priority == Priority.None)
            return DataAccessLayer.Main.AllIssuesObjects(filter.Reporter);
        else if (filter.Time != "" && filter.Condition == Condition.None && filter.Priority == Priority.None)
            return FilterByTime(filter.Time, DataAccessLayer.Main.AllIssuesObjects(filter.Reporter));
        else
            switch (filter.Time)
            {
                case "" when filter.Condition != Condition.None && filter.Priority == Priority.None:
                    return FilterByCondition(filter.Condition, DataAccessLayer.Main.AllIssues(filter.Reporter));
                case "" when filter.Condition == Condition.None && filter.Priority != Priority.None:
                    return FilterByPriority(filter.Priority, DataAccessLayer.Main.AllIssues(filter.Reporter));
                case "" when filter.Condition != Condition.None && filter.Priority != Priority.None:
                    return FilterByConditionAndPriority(filter.Condition, filter.Priority,
                        DataAccessLayer.Main.AllIssues(filter.Reporter));
                default:
                {
                    if (filter.Time != "" && filter.Condition != Condition.None && filter.Priority == Priority.None)
                        return FilterByTime(filter.Time,
                            FilterByCondition(filter.Condition, DataAccessLayer.Main.AllIssues(filter.Reporter)));
                    else if (filter.Time != "" && filter.Condition == Condition.None &&
                             filter.Priority != Priority.None)
                        return FilterByTime(filter.Time,
                            FilterByPriority(filter.Priority, DataAccessLayer.Main.AllIssues(filter.Reporter)));
                    else if (filter.Time != "" && filter.Condition != Condition.None &&
                             filter.Priority != Priority.None)
                        return FilterByTime(filter.Time,
                            FilterByConditionAndPriority(filter.Condition, filter.Priority,
                                DataAccessLayer.Main.AllIssues(filter.Reporter)));
                    break;
                }
            }

        return response;
    }

    private static List<object> FilterByTime(string time, List<object> issues)
    {
        var response = new List<object>();
        switch (time)
        {
            case "old":
                return issues;
            case "new":
                issues.Reverse();
                return issues;
            default:
                return response;
        }
    }

    private static List<object> FilterByCondition(Condition condition, List<Issue> issues)
    {
        var response = new List<object>();
        foreach (var issue in issues)
        {
            if (issue.Condition == condition)
                response.Add(issue);
        }

        return response;
    }

    private static List<object> FilterByPriority(Priority priority, List<Issue> issues)
    {
        var response = new List<object>();
        foreach (var issue in issues)
        {
            if (issue.Priority == priority)
                response.Add(issue);
        }

        return response;
    }

    private static List<object> FilterByConditionAndPriority(Condition condition, Priority priority, List<Issue> issues)
    {
        var response = new List<object>();
        var responseCondition = new List<object>();
        var responsePriority = new List<object>();
        foreach (var issue in issues)
        {
            if (issue.Condition == condition)
                responseCondition.Add(issue);
        }

        foreach (var issue in issues)
        {
            if (issue.Priority == priority)
                responsePriority.Add(issue);
        }

        foreach (var issueCondition in responseCondition)
        {
            foreach (var issuePriority in responsePriority)
            {
                if (issueCondition == issuePriority)
                    response.Add(issuePriority);
            }
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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool DeleteIssue(int id)
    {
        try
        {
            var result = DataAccessLayer.Main.DeleteIssue(id);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool CheckJwtValidation(string username, string jwt)
    {
        try
        {
            DotEnv.Load();
            var secret = DataAccessLayer.Main.GetPersonalToken(username) + Environment.GetEnvironmentVariable("SECRET_KEY");
            var issuer = Environment.GetEnvironmentVariable("ISSUER_ADDRESS") + "";
            var audience = Environment.GetEnvironmentVariable("AUDIENCE_ADDRESS") + "";
            var jwtService = new JwtService(secret, issuer, audience);
            return jwtService.VerifySecurityToken(jwt, secret, username);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? CheckVerificationEmail(string username)
    {
        try
        {
            return DataAccessLayer.Main.GetVerificationEmail(username);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string SendCodeForEmailValidation(string username)
    {
        var name = DataAccessLayer.Main.GetFullName(username);
        var email = DataAccessLayer.Main.GetEmail(username);
        if (name == null || email == null) return "User not found !";
        var sendEmail = new SendEmail(name, email);
        var code = sendEmail.SendCode();
        return DataAccessLayer.Main.UpdateEmailValidation(username, code) ? code : "false";
    }

    public static bool CheckCodeForEmailValidation(string username, string submitCode)
    {
        var verificationCode = DataAccessLayer.Main.GetVerificationEmail(username);
        var result = verificationCode != null && SendEmail.CheckCode(submitCode, verificationCode);
        if (result) DataAccessLayer.Main.UpdateEmailValidation(username, "true");
        return result;
    }
}