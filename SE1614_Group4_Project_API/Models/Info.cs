using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Info
    {
        public int Id { get; set; }
        public string? FileId { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }

        public virtual File? File { get; set; }
    }
}
