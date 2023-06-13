using TODO.Models;

namespace TODO;

public class APIS
{
    public static object signup(Signup signup) { return PresentationLayer.Main.SignupMethod(signup); }

    public static object allUsers() { return PresentationLayer.Main.AllUsersMethod(); }

    public static object login(Login login) { return PresentationLayer.Main.LoginMethod(login); }

    public static object createIssue(Issue issue) { return PresentationLayer.Main.CreateIssueMethod(issue); }

    public static object allIssues(string reporter) { return PresentationLayer.Main.AllIssuesMethod(reporter); }
            
    public static object filterIssues(Filter filter) { return PresentationLayer.Main.FilterIssuesMethod(filter); }

    public static object editIssue(Issue issue) { return PresentationLayer.Main.EditIssueMethod(issue); }

    public static object deleteIssue(int id) { return PresentationLayer.Main.DeleteIssueMethod(id); }
}