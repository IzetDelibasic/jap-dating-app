using MediatR;

public class GetTagsForPhotoQuery : IRequest<List<string>>
{
    public int PhotoId { get; set; }
}