using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class YoutubeDatum
    {
        public int Id { get; set; }
        public string? PostId { get; set; }
        public string? ChannelAvatarUrl { get; set; }
        public string? ChannelTitle { get; set; }
        public string? ChannelUrl { get; set; }
        public bool? Star { get; set; }
        public string? Thumbnail { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public int? ViewCount { get; set; }

        public virtual Post? Post { get; set; }
    }
}
