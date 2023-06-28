using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.DTOs;
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

        public int CountTotalCommentByUserId(string userId)
        {
            int count = 0;
            if (userId == null) throw new ArgumentNullException("userId");
            var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).ToList();
            foreach (var post in posts)
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

        //public Post GetAllPostsByUserId(string userId)
        //{
        //	if (userId == null) throw new ArgumentNullException("userId");
        //          var posts = _.Posts.Where(x => x.CreatorId.Equals(userId)).OrderByDescending(x => x.CreatedAt);
        //          return posts;
        //	throw new NotImplementedException();
        //}

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

        public void UpdatePostRecently(UpdatePostDTO entity)
        {
            var post = _.Posts.Find(entity.Id);
            var author = _.Users.Where(_ => _.Name.Equals(entity.Author)).Select(_ => _.Id).First();

            if (post != null)
            {
                post.Title = entity.Title;
                post.Description = entity.Description;
                post.CreatorId = author;
                post.CatId = entity.CategoryId;
                post.Slug = entity.Slug;
                _.Posts.Update(post);
                _.SaveChangesAsync();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateStatus(UpdateStatusDTO entity)
        {
            var post = _.Posts.Find(entity.Id);
            post.IsEditorPick = entity.Status;

            _.Posts.Update(post);
            _.SaveChangesAsync();

        }

        public string GetTextPost(int id)
        {
            string result = "";
            try
            {
                var post = _.Posts.Find(id);
                var blocks = _.Blocks.Include(_ => _.Datum).Where(_ => _.PostId == post.Id1).ToList();
                foreach (var item in blocks)
                {
                    result = result + item.Datum.Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public List<Post> GetPopularPosts()
        {
            return _.Posts.OrderByDescending(x => x.ViewsCount).Take(3).ToList();
            throw new NotImplementedException();
        }
    }
}