using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Unsplash
    {
        public int Id { get; set; }
        public string? DataId { get; set; }
        public string? Author { get; set; }
        public string? ProfileLink { get; set; }

        public virtual Datum? Data { get; set; }
    }
}
