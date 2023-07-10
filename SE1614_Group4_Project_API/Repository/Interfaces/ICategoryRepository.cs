using SE1614_Group4_Project_API.Models;

namespace SE1614_Group4_Project_API.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository<Cat>
    {
        List<Cat> GetTop5Category();

        Cat GetCategoryById(int cateId);
    }
}