using MediatR;

public class GetCurrentUserLikeIdsQuery : IRequest<IEnumerable<int>>
{
    public int UserId { get; set; }
}