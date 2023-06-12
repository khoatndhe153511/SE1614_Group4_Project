using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Datum
    {
        public Datum()
        {
            Unsplashes = new HashSet<Unsplash>();
        }

        public int Id { get; set; }
        public string BlockId { get; set; } = null!;
        public string? Alignment { get; set; }
        public string? Caption { get; set; }
        public bool? DockLeft { get; set; }
        public bool? DockRight { get; set; }
        public string? Embed { get; set; }
        public bool? Expanded { get; set; }
        public int? Height { get; set; }
        public int? Level { get; set; }
        public string? Link { get; set; }
        public string? Service { get; set; }
        public string? Source { get; set; }
        public bool? Stretched { get; set; }
        public string? Text { get; set; }
        public string? Url { get; set; }
        public int? Width { get; set; }

        public virtual Block Block { get; set; } = null!;
        public virtual File? File { get; set; }
        public virtual Metum? Metum { get; set; }
        public virtual ICollection<Unsplash> Unsplashes { get; set; }
    }
}
