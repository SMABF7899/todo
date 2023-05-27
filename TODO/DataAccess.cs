namespace TODO;

public abstract class DataAccess
{
    public static object AddUser(Signup signup)
    {
        object response = new { message = "Username or email is already registered", };
        using var db = new Database();
        string token = BCrypt.Net.BCrypt.HashPassword(signup.password);
        signup.personalToken = token;
        if (!db.Signups.Any())
        {
            db.Signups.Add(signup);
            db.SaveChanges();
            response = new { message = "Registration was successful", };
        }
        else
        {
            int flag = db.Signups.Count();
            foreach (var signupDb in db.Signups)
            {
                if (signup.email != signupDb.email && signup.username != signupDb.username)
                {
                    flag--;
                    if (flag == 0)
                    {
                        db.Signups.Add(signup);
                        db.SaveChanges();
                        response = new { message = "Registration was successful", };
                    }
                }
            }
        }

        return response;
    }

    public static object EnterUser(Login login)
    {
        
        object response = new { message = "Username or password is not correct", };
        using var db = new Database();
        foreach (var signup in db.Signups)
        {
            if (signup.username == login.username && signup.password == login.password && BCrypt.Net.BCrypt.Verify(login.password, signup.personalToken))
            {
                string secret = signup.personalToken + "634F74422332688E2FC348EC5B8DC";
                JwtService jwtService = new JwtService(secret, "https://smabf.ir/", "https://todo.smabf.ir");
                response = new { message = "Login was successful", jwt = jwtService.GenerateSecurityToken(login.username) };
            }
        }

        return response;
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
}