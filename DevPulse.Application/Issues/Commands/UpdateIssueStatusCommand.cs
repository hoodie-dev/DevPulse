using MediatR;
using DevPulse.Domain.Enums;
using DevPulse.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevPulse.Application.Issues.Commands;

public record UpdateIssueStatusCommand(Guid IssueId, IssueStatus NewStatus) : IRequest<bool>;

public class UpdateIssueStatusCommandHandler : IRequestHandler<UpdateIssueStatusCommand, bool>
{
    private readonly IDevPulseDbContext _context;

    public UpdateIssueStatusCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateIssueStatusCommand request, CancellationToken cancellationToken)
    {
        var issue = await _context.Issues.FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if(issue == null)
        {
            return false;
        }

        issue.Status = request.NewStatus;
        issue.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}