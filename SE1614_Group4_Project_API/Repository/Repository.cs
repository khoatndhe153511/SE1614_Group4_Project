using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Linq.Expressions;

namespace SE1614_Group4_Project_API.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly spriderumContext _spriderumContext;
        private readonly DbSet<T> _entities;

        public Repository(spriderumContext spriderumContext)
        {
            _spriderumContext = spriderumContext;
            _entities = _spriderumContext.Set<T>();
        }

        public DbSet<T> GetDbSet() => _entities;

        public T Find(params object?[]? objects)
        {
            var findResult = _entities.Find(objects);
            return findResult ?? throw new NullReferenceException("Record not found");
        }

        public virtual IEnumerable<T> FindwithQuery(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public async Task<List<T>> GetAll()
        {
            var Results = await _entities.ToListAsync();
            return Results;
        }

        public async Task Add(T entity)
        {
            _entities.Add(entity);
            await _spriderumContext.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (_spriderumContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _entities.Update(entity);
            await _spriderumContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            if (_spriderumContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _entities.Remove(entity);
            await _spriderumContext.SaveChangesAsync();
        }

        public async Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _entities.Remove(foundRecord);
            await _spriderumContext.SaveChangesAsync();
        }
    }
}