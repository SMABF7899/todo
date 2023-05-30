using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TODO.Models;

namespace TODO
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthorization();
            app.MapControllers();
            
            app.MapGet("/", () => "TODO List");
            app.MapPost("/signup", signup);
            app.MapPost("/login", login);
            app.MapGet("/allUsers", allUsers);
            app.MapPost("/createIssue", createIssue);
            app.MapGet("/allIssues", allIssues);
            app.MapPost("/editIssue", editIssue);
            app.MapPost("/deleteIssue", deleteIssue);
            app.MapGet("/healthCheck", () => "app is Up :)");

            object signup(Signup signup) { return PresentationLayer.Main.SignupMethod(signup); }

            object allUsers() { return PresentationLayer.Main.AllUsersMethod(); }

            object login(Login login) { return PresentationLayer.Main.LoginMethod(login); }

            object createIssue(Issue issue) { return PresentationLayer.Main.CreateIssueMethod(issue); }

            object allIssues() { return PresentationLayer.Main.AllIssuesMethod(); }

            object editIssue(Issue issue) { return PresentationLayer.Main.EditIssueMethod(issue); }

            object deleteIssue(int id) { return PresentationLayer.Main.DeleteIssueMethod(id); }

            app.Run();
        }
    }
}