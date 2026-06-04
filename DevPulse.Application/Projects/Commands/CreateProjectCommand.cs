using DevPulse.Domain.Entities;
using DevPulse.Application.Projects.Commands;
using MediatR;
using DevPulse.Application.Common.Interfaces;

namespace DevPulse.Application.Projects.Commands;

public record CreateProjectCommand(string Name, string Description, string Code) : IRequest<Guid>;
public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IDevPulseDbContext _context;

    public CreateProjectCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            Code = request.Code.ToUpper()
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);
        return project.Id;
    }
}
