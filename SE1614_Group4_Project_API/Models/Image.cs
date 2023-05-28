using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public int? MetaId { get; set; }
        public string? Url { get; set; }

        public virtual Metum? Meta { get; set; }
    }
}
