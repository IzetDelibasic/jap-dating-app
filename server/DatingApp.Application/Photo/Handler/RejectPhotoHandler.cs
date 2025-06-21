using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;

public class RejectPhotoHandler(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    : IRequestHandler<RejectPhotoCommand, bool>
{
    public async Task<bool> Handle(RejectPhotoCommand request, CancellationToken cancellationToken)
    {
        var photo = await unitOfWork.PhotoRepository.GetPhotoById(request.PhotoId);
        if (photo == null) return false;

        if (photo.PublicId != null)
        {
            var result = await cloudinaryService.DeletePhotoAsync(photo.PublicId);
            if (result.Result == "ok")
            {
                unitOfWork.PhotoRepository.Delete(photo);
            }
        }

        return await unitOfWork.Complete();
    }
}