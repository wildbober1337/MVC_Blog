using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Содержание")]
        public string MainText { get; set; }
        [Display(Name = "Категория")]
        public string Tag { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        [Display(Name = "#Тэги")]
        public virtual ICollection<Tag> Tags { get; set; }
        public Blog()
        {
            Tags = new List<Tag>();
        }
    }
}