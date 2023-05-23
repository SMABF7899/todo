using System.Runtime.InteropServices.JavaScript;
using TODO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "TODO List");
app.MapPost("/signup", signup);
app.MapGet("/allUsers", allUsers);

Object signup(Signup signup)
{
    using (var db = new Database())
    {
        db.Signups.Add(signup);
        db.SaveChanges();
    }

    Object response = new
    {
        message = "Registration was successful",
    };
    return response;
}

List<Object> allUsers()
{
    List<Object> response = new List<Object>();
    using (var db = new Database())
    {
        foreach (var signup in db.Signups)
        {
            response.Add(new
            {
                id = signup.id,
                firstName = signup.firstName,
                lastName = signup.lastName,
                username = signup.username,
                email = signup.email,
                password = signup.password
            });
        }
    }

    return response;
}

app.Run();