using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
        public Author()
        {
            Blogs = new List<Blog>();
        }
    }
}