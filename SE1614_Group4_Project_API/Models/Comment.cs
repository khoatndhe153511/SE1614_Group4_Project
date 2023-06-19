using System;
using System.Collections.Generic;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseIdParentNavigation = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public int? IdParent { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Comment? IdParentNavigation { get; set; }
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> InverseIdParentNavigation { get; set; }
    }
}
