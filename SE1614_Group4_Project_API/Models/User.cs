using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
        }

        public string Id { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? DisplayName { get; set; }
        public string? Gravatar { get; set; }
        public string? Name { get; set; }
        public int? Role { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}
