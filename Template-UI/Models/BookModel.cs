using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_UI.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorId { get; set; }
        public virtual AuthorModel Author { get; set; }
    }
}