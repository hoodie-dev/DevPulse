using DevPulse.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevPulse.Application.Common.Interfaces;

public interface IDevPulseDbContext
{
    DbSet<Project> Projects{get;set;}
    DbSet<Sprint> Sprints {get;set;}
    DbSet<Issue> Issues {get;set;}

    DbSet<Comment> Comments {get;set;}

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}