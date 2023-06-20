using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;
using System.Drawing;

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

		public int CountTotalCommentByUserId(string userId)
		{
            int count = 0;
			if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
            foreach(var post in posts)
            {
                count += post.CommentCount;
            }
            return count;
			throw new NotImplementedException();
		}

		public int CountTotalPostByUserId(string userId)
		{
            if (userId == null) throw new ArgumentNullException("userId");
            return _.Posts.Count(x => x.CreatorId.Equals(userId));
			throw new NotImplementedException();
		}

		public int CountTotalViewByUserId(string userId)
		{

			int count = 0;
			if (userId == null) throw new ArgumentNullException("userId");
			var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
			foreach (var post in posts)
			{
				count += post.ViewsCount;
			}
			return count;
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

		public IEnumerable<Post> GetAllPostsByUserId(string userId)
		{
			if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).Select(x => new Post
            {
                Id = x.Id,
				Id1 = x.Id1,
				CommentCount = x.CommentCount,
                CatId = x.CatId,
                ControlversialPoint = x.ControlversialPoint,
				DatePoint = x.DatePoint,
				Description = x.Description,
				CreatedAt = x.CreatedAt,
				HotPoint = x.HotPoint,
				NewTitle = x.NewTitle,
				OgImageUrl = x.OgImageUrl,
				Point = x.Point,
				Slug = x.Slug,
				Title = x.Title,
				ViewsCount = x.ViewsCount,
				Thumbnail = x.Thumbnail
			}).ToList();
            return posts;
			throw new NotImplementedException();
		}

		public new DbSet<Post> GetDbSet()
        {
            return _.Posts;
            throw new NotImplementedException();
        }

		public int TotalPointByUserId(string userId)
		{
			int count = 0;
			if (userId == null) throw new ArgumentNullException("userId");
			var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
			foreach (var post in posts)
			{
				count += post.Point;
			}
			return count;
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
