using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;

public class DeletePhotoHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    : IRequestHandler<DeletePhotoCommand, bool>
{
    public async Task<bool> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        if (user == null) return false;

        var photo = await unitOfWork.PhotoRepository.GetPhotoById(request.PhotoId);
        if (photo == null || photo.IsMain) return false;

        if (photo.PublicId != null)
        {
            var result = await cloudinaryService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }

        user.Photos.Remove(photo);
        return await unitOfWork.Complete();
    }
}