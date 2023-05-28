using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Datum
    {
        public Datum()
        {
            Files = new HashSet<File>();
            Meta = new HashSet<Metum>();
        }

        public int Id { get; set; }
        public string? BlockId { get; set; }
        public string? Alignment { get; set; }
        public string? Caption { get; set; }
        public bool? DockLeft { get; set; }
        public bool? DockRight { get; set; }
        public bool? Expanded { get; set; }
        public string? Link { get; set; }
        public bool? Stretched { get; set; }
        public string? Text { get; set; }

        public virtual Block? Block { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<Metum> Meta { get; set; }
    }
}
