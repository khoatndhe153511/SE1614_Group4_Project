using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        readonly spriderumContext _;
        public PostRepository(spriderumContext spriderumContext) : base(spriderumContext)
        {
            _ = spriderumContext;
        }
        public new Task Add(Post entity)
        {
            _.Posts.Add(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(Post entity)
        {
            _.Posts.Remove(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _.Posts.Remove(foundRecord);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }

        public new Post Find(params object?[]? objects)
        {
            var findResult = _.Posts.Find(objects);
            return findResult ?? throw new NullReferenceException("Record not found");
            throw new NotImplementedException();
        }

        public new Task<List<Post>> GetAll()
        {
            var Results = _.Posts.ToListAsync();
            return Results;
            throw new NotImplementedException();
        }

        public new DbSet<Post> GetDbSet()
        {
            return _.Posts;
            throw new NotImplementedException();
        }

        public new Task Update(Post entity)
        {
            _.Posts.Update(entity);
            _.SaveChangesAsync();
            throw new NotImplementedException();
        }
    }
}
