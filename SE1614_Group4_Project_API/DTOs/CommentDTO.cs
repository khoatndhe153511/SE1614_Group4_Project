namespace SE1614_Group4_Project_API.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? CreatedDate { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = null!;
        public string? ReplyUserId { get; set; }
        public string? imageUser { get; set; }
        public string? UserName { get; set;}
        public string? UserNameReply { get; set; }


    }
}
