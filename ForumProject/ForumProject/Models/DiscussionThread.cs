using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Models
{
    public class DiscussionThread
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Un nume pentru firul de discutie ce urmeaza a fi deschis este necesar.")]
        [StringLength(60, ErrorMessage = "Aici suntem mai permisivi, maxim 60 caractere.")]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "O descriere este necesara, pentru a fi cat mai clar firul de discutie.")]
        [StringLength(600, ErrorMessage = "Sa nu exageram, 600 de caractere sunt suficiente pentru o scurta descriere.")]
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual Subject Subject { get; set; }
        public virtual IEnumerable<SelectListItem> Subjects { get; set; }
    }
}