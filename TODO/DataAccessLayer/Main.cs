using dotenv.net;
using TODO.Models;

namespace TODO.DataAccessLayer;

public class Main
{
    private static readonly Database Db = new Database();
    public static void InsertUser(Signup signup)
    {
        Db.Signups.Add(signup);
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
}