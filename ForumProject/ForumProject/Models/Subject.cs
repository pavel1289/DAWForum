using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class Subject
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Un nume este necesar pentru orice lucru.")]
        public string Name { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<DiscussionThread> DiscussionThreads { get; set; }
    }
}