using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
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
			var findResult = _.Users.FirstOrDefault(x => x.Name.Equals(name));
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

		public User updateRoleAndActive(string _id, int role, bool active)
		{
			var findResult = _.Users.Where(x => x.Id == _id).FirstOrDefault();
            if (findResult == null) { throw new NullReferenceException("Record not found"); }
            findResult.Role = role;
            findResult.Active = active;
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

		public bool updateUserProfile(User user,UpdateUserProfile userUpdate)
		{
            if (user == null) throw new ArgumentNullException("user");
            if (userUpdate == null) throw new ArgumentException("user");

            if (user.Email == userUpdate.Email)
            {
				return updateUser(user, userUpdate);
			} else
            {
                if (checkEmail(userUpdate.Email))
                {
                    return false;
                } else
                {
					return updateUser(user, userUpdate);
				}
            }

			if (user.Name == userUpdate.UserName)
			{
				return updateUser(user, userUpdate);
			}
			else
			{
				if (checkUsername(userUpdate.UserName))
				{
					return false;
				}
				else
				{
					return updateUser(user, userUpdate);
				}
			}
			throw new NotImplementedException();
		}

        private bool updateUser(User user, UpdateUserProfile userUpdate)
        {
			user.Name = userUpdate.UserName;
			user.Avatar = userUpdate.Avatar;
			user.Email = userUpdate.Email;
			user.PhoneNumber = userUpdate.PhoneNumber;
			user.Birth = userUpdate.Birth;
			user.Gender = userUpdate.Gender;
			user.DisplayName = userUpdate.DisplayName;
			_.Users.Update(user);
			_.SaveChangesAsync();
			return true;
		}

		public User findById(string id)
        {
			if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("userId");
			var findResult = _.Users.FirstOrDefault(x => x.Id.Equals(id));
			return findResult ?? throw new NullReferenceException("Record not found");
			throw new NotImplementedException();
		}
	}
}