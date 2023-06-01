using dotenv.net;
using TODO.Models;

namespace TODO.DataAccessLayer;

public abstract class Main
{
    private static readonly Database Db = new Database();
    public static void InsertUser(Signup signup)
    {
        Db.Signups.Add(signup);
        Db.SaveChanges();
    }

    public static void InsertIssue(Issue issue)
    {
        Db.Issues.Add(issue);
        Db.SaveChanges();
    }

    public static bool FindUser(string username, string email)
    {
        var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
        var recordEmail = Db.Signups.FirstOrDefault(record => record.email == email);
        return recordUsername != null || recordEmail != null;
    }

    public static string? LoginUser(Login login)
    {
        DotEnv.Load();
        var record = Db.Signups.FirstOrDefault(record => record.username == login.username);
        var username = record?.username;
        var password = record?.password;
        var personalToken = record?.personalToken;
        if (login.username == username && login.password == password &&
            BCrypt.Net.BCrypt.Verify(login.password, personalToken))
        {
            string secret = personalToken + Environment.GetEnvironmentVariable("SECRET_KEY");
            var issuer = Environment.GetEnvironmentVariable("ISSUER_ADDRESS") + "";
            var audience = Environment.GetEnvironmentVariable("AUDIENCE_ADDRESS") + "";
            JwtService jwtService = new JwtService(secret, issuer, audience);
            var jwt = jwtService.GenerateSecurityToken(username);
            return jwt;
        }
        return "";
    }

    public static bool EditIssue(Issue issue)
    {
        var record = Db.Issues.FirstOrDefault(record => record.Id == issue.Id);
        if (record == null) return false;
        record.Summary = issue.Summary;
        record.Description = issue.Description;
        record.Priority = issue.Priority;
        record.Condition = issue.Condition;
        Db.SaveChanges();
        return true;
    }

    public static bool DeleteIssue(int id)
    {
        var record = Db.Issues.FirstOrDefault(record => record.Id == id);
        if (record == null) return false;
        Db.Remove(record);
        Db.SaveChanges();
        return true;
    }
}