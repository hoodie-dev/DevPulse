using DevPulse.Application.Issues.Commands;
using DevPulse.Application.Issues.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DevPulse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly ISender _mediator;

    public IssuesController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateIssue([FromBody] CreateIssueCommand command)
    {
        Guid issueId = await _mediator.Send(command);
        return Ok(issueId);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetIssuesByProject(Guid projectId)
    {
        var issues = await _mediator.Send(new GetIssuesByProjectIdQuery(projectId));
        return Ok(issues);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateIssueStatusDto dto)
    {
        var result = await _mediator.Send(new UpdateIssueStatusCommand(id, dto.Status));

        if (!result)
        {
            return NotFound(new{message = "Target issue could not be resolved."});
        }

        return NoContent(); // Success 204 - standard for patches
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssue(Guid id)
    {
        var success = await _mediator.Send(new DeleteIssueCommand(id));

        if(!success)
        {
            return NotFound(new{message = "The issue could not be found or has already been removed."});
        }

        return NoContent();
    }
 }

 public class UpdateIssueStatusDto
{
    public DevPulse.Domain.Enums.IssueStatus Status{get;set;}
}