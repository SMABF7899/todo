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

    public static bool LoginUser(Login login)
    {
        var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == login.username);
        var recordPassword = recordUsername?.password;
        var personalToken = recordUsername?.personalToken;
        if (recordUsername != null && recordPassword != null && BCrypt.Net.BCrypt.Verify(recordPassword, personalToken))
        {
            return true;
        }

        return false;
    }
}