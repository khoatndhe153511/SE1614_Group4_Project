using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Post
    {
        public Post()
        {
            Blocks = new HashSet<Block>();
            Comments = new HashSet<Comment>();
            RelatedCats = new HashSet<RelatedCat>();
            Tags = new HashSet<Tag>();
            YoutubeData = new HashSet<YoutubeDatum>();
        }

        public int Id { get; set; }
        public string Id1 { get; set; } = null!;
        public int? CatId { get; set; }
        public int? CommentCount { get; set; }
        public double? ControlversialPoint { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatorId { get; set; }
        public long? DatePoint { get; set; }
        public string? Description { get; set; }
        public double? HotPoint { get; set; }
        public bool? IsEditorPick { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? NewTitle { get; set; }
        public string? OgImageUrl { get; set; }
        public int? Point { get; set; }
        public double? ReadingTime { get; set; }
        public string? Slug { get; set; }
        public bool? Star { get; set; }
        public string? Thumbnail { get; set; }
        public string? Title { get; set; }
        public int? Type { get; set; }
        public int? ViewsCount { get; set; }

        public virtual Cat? Cat { get; set; }
        public virtual User? Creator { get; set; }
        public virtual ICollection<Block> Blocks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<RelatedCat> RelatedCats { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<YoutubeDatum> YoutubeData { get; set; }
    }
}
