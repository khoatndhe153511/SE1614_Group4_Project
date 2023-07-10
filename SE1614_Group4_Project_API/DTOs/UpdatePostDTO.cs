namespace SE1614_Group4_Project_API.DTOs
{
    public class UpdatePostDTO
    {
        public int Id { get; set; }
        public string PostId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string ogImageUrl { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
