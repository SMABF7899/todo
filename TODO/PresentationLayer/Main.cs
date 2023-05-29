using TODO.Models;

namespace TODO.PresentationLayer;

public abstract class Main : Validation
{
    public static IResult SignupMethod(Signup signup)
    {
        if (!NotEmpty.CheckSignup(signup)) return Results.BadRequest(new { message = "Enter the requested items" });
        if (CheckEmail(signup.email) && CheckName(signup.firstName, signup.lastName) &&
            CheckUsername(signup.username) && CheckPassword(signup.password))
            return Results.Ok(new { message = (string)BusinessLayer.Main.AddUser(signup) });
        return Results.BadRequest(new { message = "Enter the parameters correctly" });
    }

    public static IResult LoginMethod(Login login)
    {
        if (!NotEmpty.CheckLogin(login)) return Results.BadRequest(new { message = "Enter the requested items" });
        if (CheckUsername(login.username) && CheckPassword(login.password))
            return Results.Ok(new { message = (string)BusinessLayer.Main.EnterUser(login) });
        return Results.BadRequest(new { message = "Enter the parameters correctly" });
    }

    public static IResult AllUsersMethod()
    {
        return Results.Ok(new { message = BusinessLayer.Main.GetAllUsers() });
    }

    public static IResult CreateIssueMethod(Issue issue)
    {
        return NotEmpty.CheckIssue(issue)
            ? Results.Ok(new { message = (string)BusinessLayer.Main.AddIssue(issue) })
            : Results.BadRequest(new { message = "Enter the requested items" });
    }

    public static IResult AllIssuesMethod()
    {
        return Results.Ok(new { message = BusinessLayer.Main.GetAllIssue() });
    }

    public static IResult EditIssueMethod(Issue issue)
    {
        return NotEmpty.CheckIssue(issue)
            ? Results.Ok(new { message = (string)BusinessLayer.Main.EditIssue(issue) })
            : Results.BadRequest(new { message = "Enter the requested items" });
    }

    public static IResult DeleteIssueMethod(int id)
    {
        return Results.Ok(new { message = BusinessLayer.Main.DeleteIssue(id) });
    }
}