using DatingApp.Repository;
using DatingApp.Repository.Interfaces;

namespace DatingApp.Data;

public class UnitOfWork(DataContext db, IUserRepository userRepository,
    ILikesRepository likesRepository, IMessageRepository messageRepository, IPhotoRepository photoRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;

    public IMessageRepository MessageRepository => messageRepository;

    public ILikesRepository LikesRepository => likesRepository;

    public IPhotoRepository PhotoRepository => photoRepository;
    public async Task<bool> Complete()
    {
        return await db.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return db.ChangeTracker.HasChanges();
    }
}
