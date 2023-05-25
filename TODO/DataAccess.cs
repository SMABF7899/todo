namespace TODO;

public class DataAccess
{
    public static object AddUser(Signup signup)
    {
        using (var db = new Database())
        {
            db.Signups.Add(signup);
            db.SaveChanges();
        }

        Object response = new
        {
            message = "Registration was successful",
        };
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