using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE1614_Group4_Project_API.Models
{
    public partial class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = null!;
        public string? ReplyUserId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
