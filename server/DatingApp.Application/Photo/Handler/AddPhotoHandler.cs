using MediatR;
using DatingApp.Repository.Interfaces;
using DatingApp.Entities;
using AutoMapper;
using DatingApp.Services.Interfaces;
using DatingApp.Core.Entities;
using DatingApp.Application.Contracts.Responses;

public class AddPhotoHandler(
    IUnitOfWork unitOfWork,
    ICloudinaryService cloudinaryService,
    IMapper mapper
) : IRequestHandler<AddPhotoCommand, PhotoResponse?>
{
    public async Task<PhotoResponse?> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
    {
        if (request.Tags == null || !request.Tags.Any())
            throw new Exception("Tags cannot be null or empty.");

        var user = await unitOfWork.UserRepository.GetUserByUsernameAsync(request.Username);
        if (user == null) return null;

        var result = await cloudinaryService.AddPhotoAsync(request.File);
        if (result.Error != null) throw new Exception(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            AppUserId = user.Id,
        };

        unitOfWork.PhotoRepository.Add(photo);

        foreach (var tagName in request.Tags)
        {
            var tag = await unitOfWork.TagRepository.GetByNameAsync(tagName);
            if (tag == null)
                throw new Exception($"Tag '{tagName}' does not exist.");

            unitOfWork.PhotoTagRepository.Add(new PhotoTag { Photo = photo, Tag = tag });
        }

        user.Photos.Add(photo);
        if (await unitOfWork.Complete())
            return mapper.Map<PhotoResponse>(photo);

        throw new Exception("Problem adding photo with tags");
    }
}