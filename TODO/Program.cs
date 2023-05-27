using System.Runtime.InteropServices.JavaScript;
using TODO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "TODO List");
app.MapPost("/signup", signup);
app.MapPost("/login", login);
app.MapGet("/allUsers", allUsers);
app.MapPost("/createIssue", createIssue);
app.MapGet("/allIssues", allIssues);
app.MapGet("/healthCheck", () => "app is Up :)");

object signup(Signup signup) { return Main.SignupMethod(signup); }

object allUsers() { return Main.AllUsersMethod(); }

object login(Login login) { return Main.LoginMethod(login); }

object createIssue(Issue issue) { return Main.CreateIssueMethod(issue); }

object allIssues() { return Main.AllIssuesMethod(); }

app.Run();
