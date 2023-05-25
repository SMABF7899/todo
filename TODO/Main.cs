namespace TODO;

public class Main
{
    public static object SignupMethod(Signup signup) { return DataAccess.AddUser(signup); }
    
    public static List<object> AllUsersMethod() { return DataAccess.GetAllUsers(); }
}