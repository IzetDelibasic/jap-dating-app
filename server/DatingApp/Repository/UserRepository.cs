using DatingApp.Data;
using DatingApp.Entities;
using DatingApp.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository;

public class UserRepository(DataContext db) : IUserRepository
{
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await db.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await db.Users.SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await db.Users.ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await db.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        db.Entry(user).State = EntityState.Modified;
    }
}
