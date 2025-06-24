using MediatR;
using DatingApp.Repository.Interfaces;
using AutoMapper;

public class UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        
        if (user == null) return false;

        mapper.Map(request.Request, user);
        return await unitOfWork.Complete();
    }
}