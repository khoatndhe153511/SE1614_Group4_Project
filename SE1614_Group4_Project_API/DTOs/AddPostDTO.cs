namespace SE1614_Group4_Project_API.DTOs
{
    public class AddPostDTO
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string ogImageUrl { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
