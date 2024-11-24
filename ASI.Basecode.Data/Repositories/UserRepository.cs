using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASI.Basecode.Data.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<User> GetUsers()
        {
            return this.GetDbSet<User>();
        }

        public bool UserExists(string userId)
        {
            return this.GetDbSet<User>().Any(x => x.UserId == userId);
        }

        public void AddUser(User user)
        {
            this.GetDbSet<User>().Add(user); // Add the user to the DbSet
            UnitOfWork.SaveChanges(); // Save the changes to the database
        }

        public bool DeleteUserById(int id)
        {
            var user = this.GetDbSet<User>().Find(id);
            if (user != null)
            {
                this.GetDbSet<User>().Remove(user);
                UnitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        // Adjusted method to use GetDbSet<User>() instead of _context
        public User GetUserById(string userId)
        {
            return this.GetDbSet<User>().FirstOrDefault(u => u.UserId == userId);
        }

        // Adjusted method to use GetDbSet<User>() instead of _context
        public void UpdateUser(User user)
        {
            var dbSet = this.GetDbSet<User>();
            dbSet.Update(user);
            UnitOfWork.SaveChanges();
        }

        public void Add(User user)
        {
            this.GetDbSet<User>().Add(user); // Use GetDbSet<User>() to access the DbSet
            UnitOfWork.SaveChanges(); // Save changes via UnitOfWork
        }

    }
}
