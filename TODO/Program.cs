using System.Runtime.InteropServices.JavaScript;
using TODO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "TODO List");
app.MapPost("/signup", signup);
app.MapGet("/allUsers", allUsers);
app.MapGet("/healthCheck", () => "app is Up :)");

object signup(Signup signup) { return Main.SignupMethod(signup); }

object allUsers() { return Main.AllUsersMethod(); }

app.Run();
