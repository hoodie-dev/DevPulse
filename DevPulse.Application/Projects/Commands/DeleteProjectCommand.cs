using MediatR;
using Microsoft.EntityFrameworkCore;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Projects.Commands;

public record DeleteProjectCommand(Guid Id): IRequest<bool>;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IDevPulseDbContext _context;

    public DeleteProjectCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if(project == null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}