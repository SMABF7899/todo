﻿using TODO.BusinessLayer;
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
        try
        {
            var result = BusinessLayer.Main.AddUser(signup);
            return !result
                ? Results.BadRequest(new { message = "Username or email is already registered" })
                : Results.Ok(new { message = "Registration was successful" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Add User - 500" });
        }
    }

    public static IResult LoginMethod(Login login)
    {
        if (!NotEmpty.CheckLogin(login)) return Results.BadRequest(new { message = "Enter the requested items" });
        if (!CheckUsername(login.username) || !CheckPassword(login.password))
            return Results.BadRequest(new { message = "Enter the parameters correctly" });
        try
        {
            var result = BusinessLayer.Main.EnterUser(login);
            return result != ""
                ? Results.Ok(new { message = "Login was successful", jwt = result })
                : Results.BadRequest(new { message = "Username or password is not correct" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Login User - 500" });
        }
    }

    public static IResult AllUsersMethod()
    {
        return Results.Ok(new { message = BusinessLayer.Main.GetAllUsers() });
    }

    public static IResult CreateIssueMethod(Issue issue, string jwt)
    {
        if (!NotEmpty.CheckIssue(issue)) return Results.BadRequest(new { message = "Enter the requested items" });
        try
        {
            if (!BusinessLayer.Main.CheckJwtValidation(issue.Reporter, jwt))
                return Results.BadRequest(new { message = false });
            var result = BusinessLayer.Main.AddIssue(issue);
            return result
                ? Results.Ok(new { message = "Issue has been successfully created" })
                : Results.BadRequest(new { message = "Enter the requested items" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Add Issue - 500" });
        }
    }

    public static IResult AllIssuesMethod(string reporter, string jwt)
    {
        try
        {
            if (!BusinessLayer.Main.CheckJwtValidation(reporter, jwt))
                return Results.BadRequest(new { message = false });
            return BusinessLayer.Main.GetAllIssue(reporter).Count == 0
                ? Results.BadRequest(new { message = "No Issues Found !" })
                : Results.Ok(new { message = BusinessLayer.Main.GetAllIssue(reporter) });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Get Issue - 500" });
        }
    }

    public static IResult FilterIssuesMethod(Filter filter, string jwt)
    {
        if (!BusinessLayer.Main.CheckJwtValidation(filter.Reporter, jwt))
            return Results.BadRequest(new { message = false });
        return BusinessLayer.Main.GetAllIssue(filter.Reporter).Count == 0
            ? Results.BadRequest(new { message = "No Issues Found !" })
            : Results.Ok(new { message = BusinessLayer.Main.GetIssueByFilter(filter) });
    }

    public static IResult EditIssueMethod(Issue issue, string jwt)
    {
        if (!NotEmpty.CheckIssue(issue)) return Results.BadRequest(new { message = "Enter the requested items" });
        try
        {
            if (!BusinessLayer.Main.CheckJwtValidation(issue.Reporter, jwt))
                return Results.BadRequest(new { message = false });
            var result = BusinessLayer.Main.EditIssue(issue);
            return result
                ? Results.Ok(new { message = "Issue has been updated" })
                : Results.BadRequest(new { message = "Issue not found" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Edit Issue - 500" });
        }
    }

    public static IResult DeleteIssueMethod(Issue issue, string jwt)
    {
        try
        {
            if (!BusinessLayer.Main.CheckJwtValidation(issue.Reporter, jwt))
                return Results.BadRequest(new { message = "Not Login" });
            var result = BusinessLayer.Main.DeleteIssue(issue.Id);
            return result
                ? Results.Ok(new { message = "Issue has been deleted" })
                : Results.BadRequest(new { message = "Issue not found" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in Delete Issue - 500" });
        }
    }

    public static IResult CheckJWTMethod(string username, string jwt)
    {
        try
        {
            return !BusinessLayer.Main.CheckJwtValidation(username, jwt)
                ? Results.BadRequest(new { message = false })
                : Results.Ok(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(new { message = "Error in check JWT - 500" });
        }
    }
}