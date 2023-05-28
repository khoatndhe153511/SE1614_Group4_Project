using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Metum
    {
        public Metum()
        {
            Images = new HashSet<Image>();
        }

        public int Id { get; set; }
        public int? DataId { get; set; }
        public string? Description { get; set; }
        public string? Title { get; set; }

        public virtual Datum? Data { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
