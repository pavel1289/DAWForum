using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Un nume pentru categorie este necesar, ar trebui sa stii asta, dat fiind ca esti administrator.")]
        public string Name { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}