using MediatR;
using DatingApp.Repository.Interfaces;
using AutoMapper;
using DatingApp.Application.Contracts.Responses;

public class GetUserByUsernameHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    IRequestHandler<GetUserByUsernameQuery, MemberResponse?>
{
    public async Task<MemberResponse?> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
    {
        var req = await unitOfWork.UserRepository.GetMemberAsync(request.Username, request.Username == request.CurrentUsername);
        return req != null ? mapper.Map<MemberResponse>(req) : null;
    }
}