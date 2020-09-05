using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Template_UI.Models
{
    public class AuthorModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Required]
        [DisplayName("Biography")]
        [StringLength(250)]
        public string Bio { get; set; }
        public virtual IList<BookModel> Books { get; set; }
    }
}
