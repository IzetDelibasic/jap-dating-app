using DatingApp.Infrastructure.Interfaces.IRepository;

namespace DatingApp.Repository.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IMessageRepository MessageRepository { get; }
    ILikesRepository LikesRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    ITagRepository TagRepository { get; }
    IPhotoTagRepository PhotoTagRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}
