﻿using dotenv.net;
using TODO.Models;

namespace TODO.DataAccessLayer;

public abstract class Main
{
    private static readonly Database Db = new Database();

    public static bool InsertUser(Signup signup)
    {
        try
        {
            Db.Signups.Add(signup);
            Db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool InsertIssue(Issue issue)
    {
        try
        {
            Db.Issues.Add(issue);
            Db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool FindUser(string username, string email)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            var recordEmail = Db.Signups.FirstOrDefault(record => record.email == email);
            return recordUsername != null || recordEmail != null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool FindUser(string username)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            return recordUsername != null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? LoginUser(Login login)
    {
        try
        {
            DotEnv.Load();
            var record = Db.Signups.FirstOrDefault(record => record.username == login.username);
            var username = record?.username;
            var password = record?.password;
            var personalToken = record?.personalToken;
            if (login.username == username && login.password == password &&
                BCrypt.Net.BCrypt.Verify(login.password, personalToken))
            {
                var secret = personalToken + Environment.GetEnvironmentVariable("SECRET_KEY");
                var issuer = Environment.GetEnvironmentVariable("ISSUER_ADDRESS") + "";
                var audience = Environment.GetEnvironmentVariable("AUDIENCE_ADDRESS") + "";
                var jwtService = new JwtService(secret, issuer, audience);
                var jwt = jwtService.GenerateSecurityToken(username);
                return jwt;
            }

            return "";
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool EditIssue(Issue issue)
    {
        try
        {
            var record = Db.Issues.FirstOrDefault(record => record.Id == issue.Id);
            if (record == null) return false;
            record.Summary = issue.Summary;
            record.Description = issue.Description;
            record.Priority = issue.Priority;
            record.Condition = issue.Condition;
            Db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool DeleteIssue(int id)
    {
        try
        {
            var record = Db.Issues.FirstOrDefault(record => record.Id == id);
            if (record == null) return false;
            Db.Remove(record);
            Db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static List<object> AllIssuesObjects(string reporter)
    {
        var response = new List<object>();
        foreach (var issue in Db.Issues)
        {
            if (issue.Reporter == reporter)
                response.Add(issue);
        }

        return response;
    }

    public static List<Issue> AllIssues(string reporter)
    {
        var response = new List<Issue>();
        foreach (var issue in Db.Issues)
        {
            if (issue.Reporter == reporter)
                response.Add(issue);
        }

        return response;
    }

    public static string? GetPersonalToken(string username)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            return recordUsername?.personalToken;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? GetVerificationEmail(string username)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            return recordUsername?.verificationEmail;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? GetFullName(string username)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            return recordUsername?.firstName + " " + recordUsername?.lastName;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static string? GetEmail(string username)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            return recordUsername?.email;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool UpdateEmailValidation(string username, string value)
    {
        try
        {
            var recordUsername = Db.Signups.FirstOrDefault(recordUsername => recordUsername.username == username);
            if (recordUsername == null) return false;
            recordUsername.verificationEmail = value;
            Db.SaveChanges();
            return true;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}