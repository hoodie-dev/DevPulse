using MediatR;
using DevPulse.Domain.Entities;
using DevPulse.Domain.Enums;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Issues.Commands;

public record CreateIssueCommand(
    string Title,
    string Description,
    IssuePriority Priority,
    Guid ProjectId
):IRequest<Guid>;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, Guid>
{
    private readonly IDevPulseDbContext _context;
    public CreateIssueCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = new Issue
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            ProjectId = request.ProjectId,
            Status = IssueStatus.Todo
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync(cancellationToken);

        return issue.Id;
    }
}