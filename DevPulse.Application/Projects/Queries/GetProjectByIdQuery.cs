using MediatR;
using DevPulse.Domain.Entities;
using DevPulse.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevPulse.Application.Projects.Queries;

public record GetProjectByIdQuery(Guid Id): IRequest<Project?>;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Project?>
{
    private readonly IDevPulseDbContext _context;

    public GetProjectByIdQueryHandler(IDevPulseDbContext context) => _context = context;

    public async Task<Project?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
    }
}