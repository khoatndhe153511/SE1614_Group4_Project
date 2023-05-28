using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class File
    {
        public File()
        {
            Infos = new HashSet<Info>();
        }

        public int Id { get; set; }
        public int? DataId { get; set; }
        public string? Url { get; set; }

        public virtual Datum? Data { get; set; }
        public virtual ICollection<Info> Infos { get; set; }
    }
}
