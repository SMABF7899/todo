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
    
    private static string ToEmail;
    private static string ToName;
    private static int Code;

    private const string Subject = "TODO Verification Code";

    private readonly string Body = @$"<div id="":pa"" class=""a3s aiL "">
    <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""max-width:600px"">
        <tbody>
        <tr height=""16""></tr>
        <tr>
            <td>
                <table bgcolor=""#4184F3"" width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""
                       style=""min-width:332px;max-width:600px;border:1px solid #e0e0e0;border-bottom:0;border-top-left-radius:3px;border-top-right-radius:3px"">
                    <tbody>
                    <tr>
                        <td height=""72px"" colspan=""3"" style=""background-color: #127139""></td>
                    </tr>
                    <tr>
                        <td width=""32px"" style=""background-color: #127139""></td>
                        <td style=""font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:24px;color:#ffffff;line-height:1.25;background-color: #127139"">
                            TODO Verification Code
                        </td>
                        <td width=""32px"" style=""background-color: #127139""></td>
                    </tr>
                    <tr>
                        <td height=""18px"" colspan=""3"" style=""background-color: #127139""></td>
                    </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table bgcolor=""#FAFAFA"" width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""
                       style=""min-width:332px;max-width:600px;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px"">
                    <tbody>
                    <tr height=""16px"">
                        <td width=""32px"" rowspan=""3""></td>
                        <td></td>
                        <td width=""32px"" rowspan=""3""></td>
                    </tr>
                    <tr>
                        <td><p>Hello {ToName},</p>
                            <p>We received a request to access your TODO Account <span style=""color:#127139""
                                                                                         dir=""ltr""><a
                                    href=""mailto:""
                                    target=""_blank"">{ToEmail}</a></span> through your email address.
                                Your TODO verification code is:</p>
                            <div style=""text-align:center""><p dir=""ltr""><strong
                                    style=""text-align:center;font-size:24px;font-weight:bold"">{Code}</strong></p></div>
                            <p>If you did not request this code, it is possible that someone else is trying to access
                                the TODO Account <span style=""color:#127139"" dir=""ltr""><a
                                        href=""mailto:{ToEmail}"" target=""_blank"">{ToEmail}</a></span>.
                                <strong>Do not forward or give this code to anyone.</strong></p>
                            <p>You received this message because this email address is listed as the recovery email for
                                the TODO Account <span style=""color:#127139""><a
                                        href=""mailto:{ToEmail}"" target=""_blank"">{ToEmail}</a></span>.
                            </p>
                            <p>Sincerely yours,</p>
                            <p>The TODO Accounts team</p></td>
                    </tr>
                    <tr height=""32px""></tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr height=""16""></tr>
        <tr>
            <td style=""max-width:600px;font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:10px;color:#bcbcbc;line-height:1.5"">
                <table>
                    <tbody>
                    <tr>
                        <td>
                            <table style=""font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:10px;color:#666666;line-height:18px;padding-bottom:10px"">
                                <tbody>
                                <tr>
                                    <img src=""https://smabf.ir/wp-content/uploads/2023/02/SMABF.png"" height=""30"" alt=""smabf.ir""/>
                                </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        </tbody>
    </table>
</div>";

    public SendEmail(string name, string email)
    {
        ToName = name;
        ToEmail = email;
        var random = new Random();
        Code = random.Next(100000, 999999);
    }

    public void sendCode()
    {
        try
        {
            var message = new MailMessage();
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = true;
            message.From = new MailAddress(FromEmail, FromName);
            message.To.Add(new MailAddress(ToEmail, ToName));

            var smtpClient = new SmtpClient(SmtpServer, SmtpPort);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);

            smtpClient.Send(message);

            Console.WriteLine("Send OK");
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}