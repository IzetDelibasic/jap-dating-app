using MediatR;
using DatingApp.Application.Contracts.Responses;

public class GetAllTagsQuery : IRequest<IEnumerable<TagResponse>> { }