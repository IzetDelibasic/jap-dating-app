using MediatR;
using DatingApp.Repository.Interfaces;

public class SetMainPhotoHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<SetMainPhotoCommand, bool>
{
    public async Task<bool> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        if (user == null) return false;

        var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
        if (photo == null || photo.IsMain) return false;

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        return await unitOfWork.Complete();
    }
}