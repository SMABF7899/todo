namespace TODO;

public abstract class Main
{
    public static object SignupMethod(Signup signup) { return DataAccess.AddUser(signup); }
    
    public static object LoginMethod(Login login) { return DataAccess.EnterUser(login); }
    
    public static List<object> AllUsersMethod() { return DataAccess.GetAllUsers(); }
}