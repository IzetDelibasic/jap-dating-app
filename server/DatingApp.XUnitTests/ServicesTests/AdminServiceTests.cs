using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using DatingApp.Services;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DatingApp.XUnitTests.ServicesTests;

public class AdminServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPhotoRepository> _photoRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICloudinaryService> _cloudinaryServiceMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly IAdminService _adminService;

    public AdminServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _photoRepositoryMock = new Mock<IPhotoRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _cloudinaryServiceMock = new Mock<ICloudinaryService>();

        _unitOfWorkMock.Setup(u => u.PhotoRepository).Returns(_photoRepositoryMock.Object);
        _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepositoryMock.Object);

        _userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(),
            null!, null!, null!, null!, null!, null!, null!, null!);

        _adminService = new AdminService(_userManagerMock.Object, _unitOfWorkMock.Object, _cloudinaryServiceMock.Object);
    }

    private Photo CreatePhoto(int id, bool isApproved = false, bool isMain = false, int appUserId = 1)
    {
        return new Photo
        {
            Id = id,
            IsApproved = isApproved,
            IsMain = isMain,
            AppUserId = appUserId,
            Url = "https://picsum.photos/536/354"
        };
    }

    private AppUser CreateUser(int id, List<Photo> photos)
    {
        return new AppUser
        {
            Id = id,
            KnownAs = "TestUser",
            Gender = "Male",
            City = "TestCity",
            Country = "TestCountry",
            Photos = photos
        };
    }

    [Fact]
    public async Task ApprovePhoto_WhenPhotoExists_SetIsApproved()
    {
        var photo = CreatePhoto(1);
        var user = CreateUser(1, new List<Photo> { photo });

        _photoRepositoryMock.Setup(r => r.GetPhotoById(photo.Id)).ReturnsAsync(photo);
        _userRepositoryMock.Setup(r => r.GetUserByPhotoId(photo.Id)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);

        var result = await _adminService.ApprovePhoto(photo.Id);

        Assert.True(result);
        Assert.True(photo.IsApproved);
    }

    [Fact]
    public async Task ApprovePhoto_WhenUserHasNoMainPhoto_SetPhotoAsMain()
    {
        var photo = CreatePhoto(1);
        var user = CreateUser(1, new List<Photo> { photo });

        _photoRepositoryMock.Setup(r => r.GetPhotoById(photo.Id)).ReturnsAsync(photo);
        _userRepositoryMock.Setup(r => r.GetUserByPhotoId(photo.Id)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);

        var result = await _adminService.ApprovePhoto(photo.Id);

        Assert.True(result);
        Assert.True(photo.IsApproved);
        Assert.True(photo.IsMain);
    }

    [Fact]
    public async Task ApprovePhoto_WhenUserAlreadyHasMainPhoto_NotChangeMainPhoto()
    {
        var photoToApprove = CreatePhoto(1);
        var mainPhoto = CreatePhoto(2, isApproved: true, isMain: true);
        var user = CreateUser(1, new List<Photo> { mainPhoto, photoToApprove });

        _photoRepositoryMock.Setup(r => r.GetPhotoById(photoToApprove.Id)).ReturnsAsync(photoToApprove);
        _userRepositoryMock.Setup(r => r.GetUserByPhotoId(photoToApprove.Id)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);

        var result = await _adminService.ApprovePhoto(photoToApprove.Id);

        Assert.True(result);
        Assert.True(photoToApprove.IsApproved);
        Assert.False(photoToApprove.IsMain);
    }

    [Fact]
    public async Task ApprovePhoto_WhenPhotoDoesNotExist_ReturnFalse()
    {
        _photoRepositoryMock.Setup(r => r.GetPhotoById(It.IsAny<int>())).ReturnsAsync((Photo?)null);

        var result = await _adminService.ApprovePhoto(999);

        Assert.False(result);
    }

    [Fact]
    public async Task ApprovePhoto_WhenSaveFails_ReturnFalse()
    {
        var photo = CreatePhoto(1);
        var user = CreateUser(1, new List<Photo> { photo });

        _photoRepositoryMock.Setup(r => r.GetPhotoById(photo.Id)).ReturnsAsync(photo);
        _userRepositoryMock.Setup(r => r.GetUserByPhotoId(photo.Id)).ReturnsAsync(user);
        _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(false);

        var result = await _adminService.ApprovePhoto(photo.Id);

        Assert.False(result);
        Assert.True(photo.IsApproved);
    }
}
