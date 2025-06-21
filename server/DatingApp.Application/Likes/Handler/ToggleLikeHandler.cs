using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Entities;
using DatingApp.Exceptions;

public class ToggleLikeHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleLikeCommand, bool>
{
    public async Task<bool> Handle(ToggleLikeCommand request, CancellationToken cancellationToken)
    {
        if (request.SourceUserId == request.TargetUserId)
            throw new BadRequestException("You cannot like yourself");

        var existingLike = await unitOfWork.LikesRepository.GetUserLike(request.SourceUserId, request.TargetUserId);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = request.SourceUserId,
                TargetUserId = request.TargetUserId,
            };
            unitOfWork.LikesRepository.Add(like);
        }
        else
        {
            unitOfWork.LikesRepository.Delete(existingLike);
        }

        return await unitOfWork.Complete();
    }
}