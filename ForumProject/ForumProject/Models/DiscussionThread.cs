using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class DiscussionThread
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Un nume pentru firul de discutie ce urmeaza a fi deschis este necesar.")]
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}