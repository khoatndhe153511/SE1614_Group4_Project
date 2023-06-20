﻿using SE1614_Group4_Project_API.Models;

namespace SE1614_Group4_Project_API.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
		int CountTotalPostByUserId(string userId);
		int CountTotalCommentByUserId(string userId);
		int CountTotalViewByUserId(string userId);
		int TotalPointByUserId(string userId);
		IEnumerable<Post> GetAllPostsByUserId(string userId);
	}
}
