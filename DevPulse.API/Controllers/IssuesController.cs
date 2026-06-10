using DevPulse.Application.Issues.Commands;
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
 }