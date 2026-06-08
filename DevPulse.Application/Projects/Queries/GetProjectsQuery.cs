using MediatR;
using Microsoft.EntityFrameworkCore;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Projects.Queries;

public record GetProjectsQuery() : IRequest<IEnumerable<ProjectDto>>;

public record ProjectDto(Guid Id, string Name, string Code, string Description);

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IDevPulseDbContext _context;

    public GetProjectsQueryHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Projects
        .Select(p => new ProjectDto(p.Id, p.Name, p.Code, p.Description))
        .ToListAsync(cancellationToken);
    }
}