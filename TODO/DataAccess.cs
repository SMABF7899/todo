namespace TODO;

public abstract class DataAccess
{
    public static object AddUser(Signup signup)
    {
        object response = new { message = "Username or email is already registered", };
        using var db = new Database();
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
            if (signup.username == login.username && signup.password == login.password)
                response = new { message = "Login was successful", };
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