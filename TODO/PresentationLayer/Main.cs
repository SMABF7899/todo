using TODO.Models;

namespace TODO.PresentationLayer;

public abstract class Main : Validation
{
    public static IResult SignupMethod(Signup signup)
    {
        if (!NotEmpty.CheckSignup(signup)) return Results.BadRequest(new { message = "Enter the requested items" });
        if (!CheckEmail(signup.email) || !CheckName(signup.firstName, signup.lastName) ||
            !CheckUsername(signup.username) ||
            !CheckPassword(signup.password))
            return Results.BadRequest(new { message = "Enter the parameters correctly" });
        var result = BusinessLayer.Main.AddUser(signup);
        return !result
            ? Results.BadRequest(new { message = "Username or email is already registered" })
            : Results.Ok(new { message = "Registration was successful" });
    }

    public static IResult LoginMethod(Login login)
    {
        if (!NotEmpty.CheckLogin(login)) return Results.BadRequest(new { message = "Enter the requested items" });
        if (!CheckUsername(login.username) || !CheckPassword(login.password))
            return Results.BadRequest(new { message = "Enter the parameters correctly" });
        var result = BusinessLayer.Main.EnterUser(login);
        return result != ""
            ? Results.Ok(new { message = "Login was successful", jwt = result })
            : Results.BadRequest(new { message = "Username or password is not correct" });
    }

    public static IResult AllUsersMethod()
    {
        return Results.Ok(new { message = BusinessLayer.Main.GetAllUsers() });
    }

    public static IResult CreateIssueMethod(Issue issue)
    {
        if (!NotEmpty.CheckIssue(issue)) return Results.BadRequest(new { message = "Enter the requested items" });
        var result = BusinessLayer.Main.AddIssue(issue);
        return result
            ? Results.Ok(new { message = "Issue has been successfully created" })
            : Results.BadRequest(new { message = "Enter the requested items" });
    }

    public static IResult AllIssuesMethod()
    {
        return Results.Ok(new { message = BusinessLayer.Main.GetAllIssue() });
    }

    public static IResult EditIssueMethod(Issue issue)
    {
        if (!NotEmpty.CheckIssue(issue)) return Results.BadRequest(new { message = "Enter the requested items" });
        var result = BusinessLayer.Main.EditIssue(issue);
        return result
            ? Results.Ok(new { message = "Issue has been updated" })
            : Results.BadRequest(new { message = "Issue not found" });
    }

    public static IResult DeleteIssueMethod(int id)
    {
        var result = BusinessLayer.Main.DeleteIssue(id);
        return result
            ? Results.Ok(new { message = "Issue has been deleted" })
            : Results.BadRequest(new { message = "Issue not found" });
    }
}