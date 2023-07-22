using TODO.Models;
using TODO.PresentationLayer;

namespace TODO;

public class APIS
{
    public static object signup(Signup signup) { return PresentationLayer.Main.SignupMethod(signup); }

    public static object allUsers() { return PresentationLayer.Main.AllUsersMethod(); }

    public static object login(Login login) { return PresentationLayer.Main.LoginMethod(login); }

    public static object createIssue(Issue issue, string? jwt) { return PresentationLayer.Main.CreateIssueMethod(issue, jwt); }

    public static object allIssues(string reporter, string? jwt) { return PresentationLayer.Main.AllIssuesMethod(reporter, jwt); }
            
    public static object filterIssues(Filter filter, string? jwt) { return PresentationLayer.Main.FilterIssuesMethod(filter, jwt); }

    public static object editIssue(Issue issue, string? jwt) { return PresentationLayer.Main.EditIssueMethod(issue, jwt); }

    public static object deleteIssue(Issue issue, string? jwt) { return PresentationLayer.Main.DeleteIssueMethod(issue, jwt); }

    public static object checkJWT(string? username, string? jwt) { return PresentationLayer.Main.CheckJWTMethod(username, jwt); }
    public static object checkValidationEmail(string? username) { return PresentationLayer.Main.CheckValidationEmailMethod(username); }
}