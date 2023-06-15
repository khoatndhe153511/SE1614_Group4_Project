using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly spriderumContext _;

        public UserRepository(spriderumContext spriderumContext) : base(spriderumContext)
        {
            _ = spriderumContext;
        }

        public new Task Add(User entity)
        {
            _.Users.Add(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(User entity)
        {
            _.Users.Remove(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _.Users.Remove(foundRecord);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new User Find(params object?[]? objects)
        {
            var findResult = _.Users.Find(objects);
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

        public new Task<List<User>> GetAll()
        {
            var Results = _.Users.ToListAsync();
            return Results;
            throw new NotImplementedException();
        }

        public new DbSet<User> GetDbSet()
        {
            return _.Users;
            throw new NotImplementedException();
        }

        public new Task Update(User entity)
        {
            _.Users.Update(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}