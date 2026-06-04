using DevPulse.Domain.Enums;
namespace DevPulse.Domain.Entities;

public class Issue
{
    public Guid Id{get; set;} = Guid.NewGuid();
    public string Title{get;set;} = string.Empty;
    public string Description{get;set;} = string.Empty;

    public IssueStatus Status {get;set;} = IssueStatus.Todo;
    public IssuePriority Priority {get; set;} = IssuePriority.Medium;

    // foreign key and navigation for project (an issue must belong to a project)
    public Guid ProjectId {get;set;}
    public Project Project{get;set;} = null!;

    // foreign key and navigation for sprint. nullable as an issue can sit in backlog without a sprint.
    public Guid? SprintId {get;set;}
    public Sprint? Sprint {get;set;}

    // audit tracking
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public DateTime? UpdatedAt {get;set;}
 }