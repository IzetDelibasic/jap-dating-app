using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Helpers;
using AutoMapper;
using DatingApp.Application.Contracts.Responses;

public class GetUsersHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    IRequestHandler<GetUsersQuery, PagedList<MemberResponse>>
{
    public async Task<PagedList<MemberResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        request.UserParams.CurrentUsername = request.CurrentUsername;
        var pagedList = await unitOfWork.UserRepository.GetMembersAsync(request.UserParams);

        var mapped = pagedList.Select(dto => mapper.Map<MemberResponse>(dto)).ToList();

        return new PagedList<MemberResponse>(
            mapped,
            pagedList.TotalCount,
            pagedList.CurrentPage,
            pagedList.PageSize
        );
    }
}