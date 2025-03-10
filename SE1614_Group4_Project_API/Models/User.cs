﻿using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class User
    {
        public User()
        {
            Bookmarks = new HashSet<Bookmark>();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }

        public string Id { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? DisplayName { get; set; }
        public string? Gravatar { get; set; }
        public string? Name { get; set; }
        public int? Role { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? Birth { get; set; }
        public bool? Gender { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
