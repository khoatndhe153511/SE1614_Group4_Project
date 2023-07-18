using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;

namespace SE1614_Group4_Project_API.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
		User findById(string id);
		User findByName(string name);
        User updateRoleAndActive(string _id,int role, bool active);

        User FindByEmail(string email);

        void UpdatePassword(string email, string newPassword);

        bool checkUsername(string username);

        bool checkEmail(string email);

        bool updateUserProfile(User user, UpdateUserProfile userUpdate);
    }
}