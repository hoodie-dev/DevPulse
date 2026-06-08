using DevPulse.Application.Projects.Commands;
using DevPulse.Application.Projects.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ISender _mediator;

    public ProjectsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _mediator.Send(new GetProjectsQuery());
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
    {
        Guid projectId = await _mediator.Send(command);

        return Ok(projectId);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteProject(Guid Id)
    {
        bool isDeleted = await _mediator.Send(new DeleteProjectCommand(Id));

        if(!isDeleted)
        {
            return NotFound(new {message = "Project not found in database."});
        }

        return NoContent();
    }
}