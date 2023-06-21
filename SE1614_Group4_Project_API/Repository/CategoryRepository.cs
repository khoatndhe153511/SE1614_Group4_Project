using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Repository
{
    public class CategoryRepository : Repository<Cat>, ICategoryRepository
    {
        private readonly spriderumContext _context;

        public CategoryRepository(spriderumContext spriderumContext) : base(spriderumContext)
        {
            _context = spriderumContext;
        }

        public new Task Add(Cat entity)
        {
            _context.Cats.Add(entity);
            _context.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(Cat entity)
        {
            _context.Cats.Remove(entity);
            _context.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _context.Cats.Remove(foundRecord);
            _context.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Cat Find(params object?[]? objects)
        {
            var findResult = _context.Cats.Find(objects);
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

        public new Task<List<Cat>> GetAll()
        {
            var Results = _context.Cats.ToListAsync();
            return Results;
            throw new NotImplementedException();
        }

        public List<Cat> GetTop5Category()
        {
            return _context.Cats.Take(5).ToList();
        }

        public new DbSet<Cat> GetDbSet()
        {
            return _context.Cats;
            throw new NotImplementedException();
        }

        public new Task Update(Cat entity)
        {
            _context.Cats.Update(entity);
            _context.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public Cat GetCategoryById(int cateId)
        {
            return _context.Cats.FirstOrDefault(x => x.Id == cateId);
            throw new NotImplementedException();
        }
    }
}