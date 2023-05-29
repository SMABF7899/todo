namespace TODO;

public class Issue
{
    public int Id { get; set; }
    public string Summary { get; set; }
    public string Reporter { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; } = Priority.LOW;
    public Condition Condition { get; set; } = Condition.ToDo;
}