using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Block
    {
        public int Id { get; set; }
        public int? V { get; set; }
        public string Id1 { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public string? PostId { get; set; }
        public int? Status { get; set; }
        public string? Type { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Post? Post { get; set; }
        public virtual Datum? Datum { get; set; }
    }
}
