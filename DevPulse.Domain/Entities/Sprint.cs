namespace DevPulse.Domain.Entities;

public class Sprint
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Name {get; set;} = string.Empty;
    public DateTime StartDate {get;set;}
    public DateTime EndDate {get;set;}

    // foreign key - links sprint to a specific project
    public Guid ProjectId {get;set;}

    // navigation property - allows EF core to fetch full project object if needed
    public Project Project{get;set;} = null!;
}