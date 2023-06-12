using dotenv.net;
using TODO.Models;

namespace TODO
{
    public class Program
    {
        public static void Main(String[] args)
        {
            DotEnv.Load();
            var builder = WebApplication.CreateBuilder(args);
            var myAllowSpecificOrigins = Environment.GetEnvironmentVariable("SECRET_KEY_FOR_ALLOW_CLIENT") + "";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins(Environment.GetEnvironmentVariable("CLIENT_IP") + "").AllowAnyHeader()
                            .AllowAnyMethod().AllowAnyOrigin();
                    });
            });
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(myAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapControllers();

            app.MapGet("/", () => "TODO List");
            app.MapPost("/signup", signup);
            app.MapPost("/login", login);
            app.MapGet("/allUsers", allUsers);
            app.MapPost("/createIssue", createIssue);
            app.MapPost("/allIssues", allIssues);
            app.MapPost("/editIssue", editIssue);
            app.MapPost("/deleteIssue", deleteIssue);
            app.MapGet("/healthCheck", () => "app is Up :)");

            object signup(Signup signup)
            {
                return PresentationLayer.Main.SignupMethod(signup);
            }

            object allUsers()
            {
                return PresentationLayer.Main.AllUsersMethod();
            }

            object login(Login login)
            {
                return PresentationLayer.Main.LoginMethod(login);
            }

            object createIssue(Issue issue)
            {
                return PresentationLayer.Main.CreateIssueMethod(issue);
            }

            object allIssues(string reporter)
            {
                return PresentationLayer.Main.AllIssuesMethod(reporter);
            }

            object editIssue(Issue issue)
            {
                return PresentationLayer.Main.EditIssueMethod(issue);
            }

            object deleteIssue(int id)
            {
                return PresentationLayer.Main.DeleteIssueMethod(id);
            }

            app.Run();
        }
    }
}