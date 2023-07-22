namespace SE1614_Group4_Project_API.DTOs
{
    public class PostResponseDTO
    {
        public int id { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
        public bool? isEditorPick { get; set; }
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int? ViewsCount { get; set; }
    }
}
