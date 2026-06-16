using MediatR;
using DevPulse.Application.Common.Interfaces;
using DevPulse.Domain.Entities;

namespace DevPulse.Application.Issues.Commands;

public record AddCommentCommand(Guid IssueId, string Text, string Author): IRequest<CommentDto>;

public record CommentDto(Guid Id, string Text, string Author, DateTime CreatedAt);

public class AddCommentCommandHandler: IRequestHandler<AddCommentCommand, CommentDto>
{
    private readonly IDevPulseDbContext _context;

    public AddCommentCommandHandler(IDevPulseDbContext context)
    {
        _context = context;
    }

    public async Task<CommentDto> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            IssueId = request.IssueId,
            Text = request.Text,
            Author = request.Author,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new CommentDto(comment.Id, comment.Text, comment.Author, comment.CreatedAt);
    }
}