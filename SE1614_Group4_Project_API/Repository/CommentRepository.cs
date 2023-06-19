using Microsoft.EntityFrameworkCore;
using SE1614_Group4_Project_API.Models;
using SE1614_Group4_Project_API.Repository.Interfaces;

namespace SE1614_Group4_Project_API.Repository
{
	public class CommentRepository : Repository<Comment>, ICommentRepository
	{
		private readonly spriderumContext _context;

		public CommentRepository(spriderumContext spriderumContext) : base(spriderumContext)
		{
			_context = spriderumContext;
		}

		public new Task Add(Comment entity)
		{
			_context.Comments.Add(entity);
			_context.SaveChangesAsync();
			throw new NotImplementedException();
		}

		public new Task Delete(Comment entity)
		{
			_context.Comments.Remove(entity);
			_context.SaveChangesAsync();
			throw new NotImplementedException();
		}

		public new Task Delete(params object?[]? key)
		{
			var foundRecord = Find(key);
			_context.Comments.Remove(foundRecord);
			_context.SaveChangesAsync();
			throw new NotImplementedException();
		}

		public new Comment Find(params object?[]? objects)
		{
			var findResult = _context.Comments.Find(objects);
			return findResult ?? throw new NullReferenceException("Record not found");
			throw new NotImplementedException();
		}

		public new Task<List<Comment>> GetAll()
		{
			var Results = _context.Comments.ToListAsync();
			return Results;
			throw new NotImplementedException();
		}

		public new DbSet<Comment> GetDbSet()
		{
			return _context.Comments;
			throw new NotImplementedException();
		}

		public new Task Update(Comment entity)
		{
			_context.Comments.Update(entity);
			_context.SaveChangesAsync();
			throw new NotImplementedException();
		}
	}
}
