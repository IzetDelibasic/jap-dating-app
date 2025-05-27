using DatingApp.Infrastructure.Interfaces.IRepository;
using DatingApp.Repository.Interfaces;

namespace DatingApp.Data;

public class UnitOfWork(DatabaseContext db, IUserRepository userRepository,
    ILikesRepository likesRepository, IMessageRepository messageRepository,
    IPhotoRepository photoRepository, ITagRepository tagRepository, IPhotoTagRepository photoTagRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;
    public IMessageRepository MessageRepository => messageRepository;
    public ILikesRepository LikesRepository => likesRepository;
    public IPhotoRepository PhotoRepository => photoRepository;
    public ITagRepository TagRepository => tagRepository;
    public IPhotoTagRepository PhotoTagRepository => photoTagRepository;

    public async Task<bool> Complete()
    {
        return await db.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return db.ChangeTracker.HasChanges();
    }
}
