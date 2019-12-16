using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Un comentariu trebuie sa aiba text.")]
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual DiscussionThread DiscussionThread { get; set; }
    }
}