﻿using dotenv.net;
using TODO.Models;

namespace TODO.BusinessLayer;

public abstract class Main
{
    public static IResult AddUser(Signup signup)
    {
        if (DataAccessLayer.Main.FindUser(signup.username, signup.email))
            return Results.BadRequest(new { message = "Username or email is already registered" });
        var token = BCrypt.Net.BCrypt.HashPassword(signup.password);
        signup.personalToken = token;
        DataAccessLayer.Main.InsertUser(signup);
        return Results.Ok(new { message = "Registration was successful" });
    }

    public static IResult EnterUser(Login login)
    {
        DotEnv.Load();
        return DataAccessLayer.Main.LoginUser(login) != ""
            ? Results.Ok(new { message = "Login was successful", jwt = DataAccessLayer.Main.LoginUser(login) })
            : Results.BadRequest(new { message = "Username or password is not correct" });
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
                personalToken = signup.personalToken,
                firstName = signup.firstName,
                lastName = signup.lastName,
                username = signup.username,
                email = signup.email,
                password = signup.password
            });
        }

        return response;
    }

    public static IResult AddIssue(Issue issue)
    {
        DataAccessLayer.Main.InsertIssue(issue);
        return Results.Ok(new { message = "Issue has been successfully created" });
    }

    public static List<object> GetAllIssue()
    {
        List<object> response = new List<object>();
        using var db = new Database();
        foreach (var issue in db.Issues)
        {
            response.Add(new
            {
                id = issue.Id,
                summary = issue.Summary,
                reporter = issue.Reporter,
                description = issue.Description,
                priority = issue.Priority,
                condition = issue.Condition
            });
        }

        return response;
    }

    public static IResult EditIssue(Issue newIssue)
    {
        return DataAccessLayer.Main.EditIssue(newIssue)
            ? Results.Ok(new { message = "Issue has been updated" })
            : Results.BadRequest(new { message = "Issue not found" });
    }

    public static IResult DeleteIssue(int id)
    {
        return DataAccessLayer.Main.DeleteIssue(id)
            ? Results.Ok(new { message = "Issue has been deleted" })
            : Results.BadRequest(new { message = "Issue not found" });
    }
}