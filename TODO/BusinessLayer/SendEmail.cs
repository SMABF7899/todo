using System.Net;
using System.Net.Mail;

namespace TODO.BusinessLayer;

public class SendEmail
{
    private const string SmtpServer = "smtp.gmail.com";
    private const int SmtpPort = 587;
    private const string SmtpUsername = "smabf.dev";
    private const string SmtpPassword = "fvqelvnfcpcejlmn";

    private const string FromEmail = "smabf.dev@gmail.com";
    private const string FromName = "SMABF";

    private string email;
    private string name;
    private readonly int code;

    private const string Subject = "TODO Verification Code";

    public SendEmail(string name, string email)
    {
        this.name = name;
        this.email = email;
        var random = new Random();
        code = random.Next(100000, 999999);
    }

    public string SendCode()
    {
        try
        {
            var message = new MailMessage();
            var body =
                "<img src=\"https://smabf.ir/wp-content/uploads/2023/02/SMABF.png\" height=\"30\" alt=\"smabf.ir\"/>" +
                "<div>Name : " + name + "</div>" +
                "<div>Email : " + email + "</div>" +
                "<div>Code : " + code + "</div>";
            message.Subject = Subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.From = new MailAddress(FromEmail, FromName);
            message.To.Add(new MailAddress(email, name));

            var smtpClient = new SmtpClient(SmtpServer, SmtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            smtpClient.Send(message);
            Console.WriteLine("Send OK");
            return code.ToString();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static bool CheckCode(string submitCode, string verificationCode)
    {
        return submitCode == verificationCode;
    }
}