namespace TODO.Models;

public class Filter
{
    public string Reporter { get; set; }
    public string Time { get; set; }
    public Priority Priority { get; set; } = Priority.None;
    public Condition Condition { get; set; } = Condition.None;
}