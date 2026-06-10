using MediatR;
using Microsoft.EntityFrameworkCore;
using DevPulse.Domain.Entities;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Issues.Queries;

public record GetIssuesByProjectIdQuery(Guid ProjectId) : IRequest<List<Issue>>;

public class GetIssuesByProjectIdQueryHandler : IRequestHandler<GetIssuesByProjectIdQuery, List<Issue>>
{
    private readonly IDevPulseDbContext _context;

    public GetIssuesByProjectIdQueryHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<List<Issue>> Handle(GetIssuesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Issues
        .Where(i => i.ProjectId == request.ProjectId)
        .OrderByDescending(i => i.CreatedAt)
        .ToListAsync(cancellationToken);
    }
}