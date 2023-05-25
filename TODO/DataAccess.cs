namespace TODO;

public class DataAccess
{
    public static object AddUser(Signup signup)
    {
        Object response = new { message = "", };
        using (var db = new Database())
        {
            foreach (var SignupDb in db.Signups)
            {
                if (signup.email != SignupDb.email && signup.username != SignupDb.username)
                {
                    db.Signups.Add(signup);
                    db.SaveChanges();
                    response = new { message = "Registration was successful", };
                }
                else
                    response = new { message = "Username or email is already registered", };
            }
        }

        return response;
    }

    public static List<object> GetAllUsers()
    {
        List<object> response = new List<object>();
        using (var db = new Database())
        {
            foreach (var signup in db.Signups)
            {
                response.Add(new
                {
                    id = signup.id,
                    firstName = signup.firstName,
                    lastName = signup.lastName,
                    username = signup.username,
                    email = signup.email,
                    password = signup.password
                });
            }
        }

        return response;
    }
}