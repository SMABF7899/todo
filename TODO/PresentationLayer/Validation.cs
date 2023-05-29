using TODO.Models;

namespace TODO.PresentationLayer;

public abstract class Validation
{
    private static bool CheckNotNumber(string input)
    {
        var numbers = new[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        return numbers.All(number => input.IndexOf(number) != -1);
    }

    private static bool CheckNotIllegalCharacters(string input)
    {
        var illegalCharacters = new[] { '#', '$', '%', '&', '*', '+', '/', '=', '?', '\'', '@' };
        return illegalCharacters.All(illegalCharacter => input.IndexOf(illegalCharacter) != -1);
    }

    protected static bool CheckEmail(string email)
    {
        if (email.IndexOf('@') == -1) return false;
        var local = email.Split('@')[0];
        var domain = email.Split('@')[1];
        return CheckNotIllegalCharacters(local) && CheckNotIllegalCharacters(domain);
    }

    protected static bool CheckName(string firstName, string lastName)
    {
        return CheckNotIllegalCharacters(firstName) && CheckNotIllegalCharacters(lastName) &&
               CheckNotNumber(firstName) &&
               CheckNotNumber(lastName);
    }

    protected static bool CheckUsername(string username)
    {
        return CheckNotIllegalCharacters(username);
    }

    protected static bool CheckPassword(string password)
    {
        return password.Length >= 8;
    }

    protected abstract class NotEmpty
    {
        public static bool CheckSignup(Signup signup)
        {
            return signup.firstName != "" && signup.lastName != "" && signup.username != "" && signup.email != "" &&
                   signup.password != "";
        }

        public static bool CheckLogin(Login login)
        {
            return login.username != "" && login.password != "";
        }

        public static bool CheckIssue(Issue issue)
        {
            return issue.Summary != "" && issue.Description != "" && issue.Reporter != "";
        }
    }
}