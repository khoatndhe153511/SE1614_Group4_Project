using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class RelatedCat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PostId { get; set; }
        public string? Slug { get; set; }

        public virtual Post? Post { get; set; }
    }
}
