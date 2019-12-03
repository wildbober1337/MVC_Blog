using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Models
{
    public class BlogsListViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Tags { get; set; }
    }
}