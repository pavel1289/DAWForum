using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ForumProject.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Un nume este necesar pentru orice lucru.")]
        [StringLength(20, ErrorMessage = "Din nou, un nume nu ar trebui sa depaseasca 20 caractere.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Un subiect trebuie sa faca parte dintr-o categorie")]
        public int CategoryId { get; set; }
        public virtual ICollection<DiscussionThread> DiscussionThreads { get; set; }

        public virtual Category Category { get; set; }
        public virtual IEnumerable<SelectListItem> Categories { get; set; }
    }
}