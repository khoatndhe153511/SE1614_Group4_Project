using SE1614_Group4_Project_API.DTOs;
using SE1614_Group4_Project_API.Models;
using System.ComponentModel;

namespace SE1614_Group4_Project_API.Repository.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        int CountTotalPostByUserId(string userId);

        int CountTotalCommentByUserId(string userId);

        int CountTotalViewByUserId(string userId);

        int TotalPointByUserId(string userId);

        public List<PostResponseDTO> GetAllPostByUserId(string userId);
        public void UpdatePostRecently(UpdatePostDTO entity);
        public void AddPostRecently(AddPostDTO entity);
        public List<PostResponseDTO> GetPostsRecently();
        public void UpdateStatus(UpdateStatusDTO entity);

        public string GetTextPost(int id);

        List<Post> GetPopularPosts();

        List<Post> SearchPosts(string title);
        public RateResponseDTO GetRate(int id);
        public bool? GetRatesbyUserId (int postId, string userId);
        public void UpdateRate(int postId, string userId, bool? like);
    }
}