using MediatR;
using DevPulse.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevPulse.Application.Issues.Commands;

public record DeleteIssueCommand(Guid IssueId): IRequest<bool>;

public class DeleteIssueCommandHandler: IRequestHandler<DeleteIssueCommand, bool>
{
    private readonly IDevPulseDbContext _context;

    public DeleteIssueCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await _context.Issues.FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken);

        if(issue == null)
        {
            return false;
        }

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}