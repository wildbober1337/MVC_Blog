using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}