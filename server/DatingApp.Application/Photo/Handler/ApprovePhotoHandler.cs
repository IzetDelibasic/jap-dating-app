using MediatR;
using DatingApp.Repository.Interfaces;

public class ApprovePhotoHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ApprovePhotoCommand, bool>
{
    public async Task<bool> Handle(ApprovePhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = await unitOfWork.PhotoRepository.GetPhotoById(request.PhotoId);
        if (photo == null) return false;

        photo.IsApproved = true;

        var user = await unitOfWork.UserRepository.GetUserByPhotoId(request.PhotoId);
        if (user == null) return false;

        if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

        return await unitOfWork.Complete();
    }
}