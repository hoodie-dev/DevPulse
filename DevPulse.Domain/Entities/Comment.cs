namespace DevPulse.Domain.Entities;

public class Comment
{
    public Guid Id{get; set;} = Guid.NewGuid();
    public string Text {get;set;} = string.Empty;
    public string Author {get;set;} = "Anonymous User";
    public DateTime CreatedAt{get;set;} = DateTime.UtcNow;


    public Guid IssueId {get;set;}
    public Issue Issue {get;set;} = null!;
}