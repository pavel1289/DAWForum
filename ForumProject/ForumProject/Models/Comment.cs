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
        public int Id { get; set; }
        [Required(ErrorMessage = "Un comentariu trebuie sa aiba text.")]
        [StringLength(600, ErrorMessage = "Structureaza-ti comentariul in mai multe daca are peste 600 caractere.")]
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int DiscussionThreadId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual DiscussionThread DiscussionThread { get; set; }
    }
}