namespace TODO.Models;

public class Issue
{
    public int Id { get; set; }
    public string Summary { get; set; }
    public string Reporter { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; } = Priority.Low;
    public Condition Condition { get; set; } = Condition.ToDo;
}