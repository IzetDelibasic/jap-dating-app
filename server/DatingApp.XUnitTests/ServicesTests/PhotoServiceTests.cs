using CloudinaryDotNet.Actions;
using DatingApp.Entities;
using DatingApp.Infrastructure.Interfaces.IServices;
using DatingApp.Repository.Interfaces;
using DatingApp.Services.Interfaces;
using DatingApp.Services.Services;
using Moq;

namespace DatingApp.XUnitTests.ServicesTests;

public class PhotoServiceTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock;
    private readonly Mock<IPhotoRepository> photoRepositoryMock;
    private readonly Mock<IUserRepository> userRepositoryMock;
    private readonly Mock<ICloudinaryService> cloudinaryServiceMock;
    private readonly IPhotoService photoService;

    public PhotoServiceTests()
    {
        unitOfWorkMock = new Mock<IUnitOfWork>();
        photoRepositoryMock = new Mock<IPhotoRepository>();
        userRepositoryMock = new Mock<IUserRepository>();
        cloudinaryServiceMock = new Mock<ICloudinaryService>();

        unitOfWorkMock.Setup(p => p.PhotoRepository).Returns(photoRepositoryMock.Object);
        unitOfWorkMock.Setup(u => u.UserRepository).Returns(userRepositoryMock.Object);

        photoService = new PhotoService(unitOfWorkMock.Object, null!, cloudinaryServiceMock.Object);
    }

    private Photo CreatePhoto(int id, bool isMain = false, bool isApproved = true, int appUserId = 1, string? publicId = null)
    {
        return new Photo
        {
            Id = id,
            IsMain = isMain,
            IsApproved = isApproved,
            AppUserId = appUserId,
            PublicId = publicId,
            Url = $"https://picsum.photos/photo{id}.jpg"
        };
    }

    private AppUser CreateUser(int id, string username, List<Photo> photos)
    {
        return new AppUser
        {
            Id = id,
            UserName = username,
            KnownAs = "TestUser",
            Gender = "Male",
            City = "TestCity",
            Country = "TestCountry",
            Photos = photos
        };
    }

    [Fact]
    public async Task Given_PhotoBelongsToUser_When_DeletePhoto_Then_DeletePhotoSuccessfully()
    {
        var username = "testinguser";
        var photo = CreatePhoto(1, isMain: false, publicId: "publicId");
        var user = CreateUser(1, username, new List<Photo> { photo });

        userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        photoRepositoryMock.Setup(r => r.GetPhotoById(photo.Id)).ReturnsAsync(photo);
        cloudinaryServiceMock.Setup(c => c.DeletePhotoAsync(photo.PublicId!)).ReturnsAsync(new DeletionResult { Result = "ok" });
        unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);

        var result = await photoService.DeletePhoto(username, photo.Id);

        Assert.True(result);
        photoRepositoryMock.Verify(r => r.GetPhotoById(photo.Id), Times.Once);
        userRepositoryMock.Verify(r => r.GetUserByUsernameAsync(username), Times.Once);
        cloudinaryServiceMock.Verify(c => c.DeletePhotoAsync(photo.PublicId!), Times.Once);
        unitOfWorkMock.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task Given_PhotoIsMain_When_DeletePhoto_Then_ReturnFalse()
    {
        var username = "testinguser";
        var photo = CreatePhoto(1, isMain: true);
        var user = CreateUser(1, username, new List<Photo> { photo });

        userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        photoRepositoryMock.Setup(r => r.GetPhotoById(photo.Id)).ReturnsAsync(photo);

        var result = await photoService.DeletePhoto(username, photo.Id);

        Assert.False(result);
        photoRepositoryMock.Verify(r => r.GetPhotoById(photo.Id), Times.Once);
        userRepositoryMock.Verify(r => r.GetUserByUsernameAsync(username), Times.Once);
        unitOfWorkMock.Verify(u => u.Complete(), Times.Never);
    }

    [Fact]
    public async Task Given_PhotoDoesNotExist_When_DeletePhoto_Then_ReturnFalse()
    {
        var username = "testinguser";
        var photoId = 1;

        userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(CreateUser(1, username, new List<Photo>()));
        photoRepositoryMock.Setup(r => r.GetPhotoById(photoId)).ReturnsAsync((Photo?)null);

        var result = await photoService.DeletePhoto(username, photoId);

        Assert.False(result);
    }

    [Fact]
    public async Task Given_UserDoesNotExist_When_DeletePhoto_Then_ReturnFalse()
    {
        var username = "testinguser";
        var photoId = 1;

        userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync((AppUser?)null);

        var result = await photoService.DeletePhoto(username, photoId);

        Assert.False(result);
        userRepositoryMock.Verify(r => r.GetUserByUsernameAsync(username), Times.Once);
        photoRepositoryMock.Verify(r => r.GetPhotoById(photoId), Times.Never);
        unitOfWorkMock.Verify(u => u.Complete(), Times.Never);
    }
}