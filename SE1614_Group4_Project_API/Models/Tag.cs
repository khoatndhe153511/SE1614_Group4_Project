using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Tag
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? PostId { get; set; }

        public virtual Post? Post { get; set; }
    }
}
