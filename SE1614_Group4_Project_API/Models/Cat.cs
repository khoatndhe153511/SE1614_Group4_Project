using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Cat
    {
        public Cat()
        {
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
