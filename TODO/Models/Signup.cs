namespace TODO.Models;

public class Signup
{
    public int Id { get; set; }
    public string personalToken { get; set; } = String.Empty;
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
}