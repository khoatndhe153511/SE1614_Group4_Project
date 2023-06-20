using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Xml.Linq;

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

        public User findByName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("userId");
			var findResult = _.Users.FirstOrDefault(x => x.Name == name);
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

		public User updateRole(string username, int role)
		{
			var findResult = _.Users.Where(x => x.Name == username).FirstOrDefault();
            if (findResult == null) { throw new NullReferenceException("Record not found"); }
            findResult.Role = role;
            _.Users.UpdateRange(findResult);
            _.SaveChanges();
            return findResult;
		}


        public User FindByEmail(string email)
        {
            var findResult = _.Users.FirstOrDefault(x => x.Email == email);
            return findResult;
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

        public void UpdatePassword(string email, string newPass)
        {
            var user = _.Users.FirstOrDefault(x => x.Email == email);

            if (user != null)
            {
                user.Password = newPass;
                _.SaveChanges();
            }
        }

		public bool checkUsername(string username)
        {
			if (string.IsNullOrEmpty(username)) throw new ArgumentNullException("username");
			var user = _.Users.FirstOrDefault(x => x.Name.Equals(username));
            if (user != null) return true;
            else return false;
			throw new NotImplementedException();
		}

		public bool checkEmail(string email)
		{
			if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("email");
			var user = _.Users.FirstOrDefault(x => x.Email.Equals(email));
			if (user != null) return true;
			else return false;
			throw new NotImplementedException();
		}
	}
}