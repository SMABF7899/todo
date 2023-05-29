﻿using dotenv.net;
using TODO.Models;

namespace TODO.BusinessLayer;

public abstract class Main
{
    public static object AddUser(Signup signup)
    {
        object response = new { message = "Username or email is already registered", };
        using var db = new Database();
        string token = BCrypt.Net.BCrypt.HashPassword(signup.password);
        signup.personalToken = token;
        if (!db.Signups.Any())
        {
            db.Signups.Add(signup);
            db.SaveChanges();
            response = new { message = "Registration was successful", };
        }
        else
        {
            int flag = db.Signups.Count();
            foreach (var signupDb in db.Signups)
            {
                if (signup.email != signupDb.email && signup.username != signupDb.username)
                {
                    flag--;
                    if (flag == 0)
                    {
                        db.Signups.Add(signup);
                        db.SaveChanges();
                        response = new { message = "Registration was successful", };
                    }
                }
            }
        }

        return response;
    }

    public static object EnterUser(Login login)
    {
        DotEnv.Load();
        object response = new { message = "Username or password is not correct", };
        using var db = new Database();
        foreach (var signup in db.Signups)
        {
            if (signup.username == login.username && signup.password == login.password &&
                BCrypt.Net.BCrypt.Verify(login.password, signup.personalToken))
            {
                string secret = signup.personalToken + Environment.GetEnvironmentVariable("SECRET_KEY");
                JwtService jwtService = new JwtService(secret, "https://smabf.ir/", "https://todo.smabf.ir");
                response = new
                    { message = "Login was successful", jwt = jwtService.GenerateSecurityToken(login.username) };
            }
        }

        return response;
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

    public static object AddIssue(Issue issue)
    {
        using var db = new Database();
        db.Issues.Add(issue);
        db.SaveChanges();
        object response = new { message = "Issue has been successfully created" };
        return response;
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

    public static object EditIssue(Issue newIssue)
    {
        object response = new { message = "Issue not found", };
        using var db = new Database();
        foreach (var issue in db.Issues)
        {
            if (newIssue.Id == issue.Id)
            {
                issue.Summary = newIssue.Summary;
                issue.Description = newIssue.Description;
                issue.Priority = newIssue.Priority;
                issue.Condition = newIssue.Condition;
                db.SaveChanges();
                response = new { message = "Issue has been updated", };
            }
        }

        return response;
    }

    public static object DeleteIssue(int Id)
    {
        object response = new { message = "Issue not found", };
        using var db = new Database();
        foreach (var issue in db.Issues)
        {
            if (Id == issue.Id)
            {
                db.Issues.Remove(issue);
                db.SaveChanges();
                response = new { message = "Issue has been deleted", };
            }
        }

        return response;
    }
}