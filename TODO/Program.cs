using dotenv.net;
using TODO.Models;

namespace TODO
{
    public abstract class Program
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
            app.MapPost("/signup", APIS.signup);
            app.MapPost("/login", APIS.login);
            app.MapGet("/allUsers", APIS.allUsers);
            app.MapPost("/createIssue", APIS.createIssue);
            app.MapPost("/allIssues", APIS.allIssues);
            app.MapPost("/filterIssues", APIS.filterIssues);
            app.MapPost("/editIssue", APIS.editIssue);
            app.MapPost("/deleteIssue", APIS.deleteIssue);
            app.MapPost("/checkJWT", APIS.checkJWT);
            app.MapPost("/checkValidationEmail", APIS.checkValidationEmail);
            app.MapPost("/sendCodeForEmailValidation", APIS.sendCodeForEmailValidation);
            app.MapPost("/checkCodeForEmailValidation", APIS.checkCodeForEmailValidation);
            app.MapGet("/buildDate", () => Environment.GetEnvironmentVariable("BUILD_DATE"));
            app.Run();
        }
    }
}