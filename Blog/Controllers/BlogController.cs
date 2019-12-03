using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Security;
using System.Text;
using System.Web;

namespace Blog.Controllers
{
    public class BlogController : Controller
    {
        BlogContext db = new BlogContext();
        SelectList Categories = new SelectList(new List<string>()
            {
                "Все",
                "Программирование",
                "Культура и Здоровье",
                "Досуг и развлечения",
                "Бизнес и Финансы"
            });

        public ActionResult Index(string categories, string tag)
        {
            List<Author> authors = db.Authors.ToList();
            IQueryable<Models.Blog> blogs = db.Blogs.Include(p => p.Author);
            List<Tag> Tags = db.Tags.ToList();
            List<Models.Blog> Blogs = db.Blogs.ToList();
            List<string> tag_name = new List<string>();
            for (int i = 0; i < Tags.Count(); i++)
            {
                tag_name.Add(Tags[i].Name);
            }
            tag_name.Add("Все");
            SelectList tags = new SelectList(tag_name);
            List<Models.Blog> Blogs_d = new List<Models.Blog>();
            Blogs_d = blogs.ToList();


            if (!String.IsNullOrEmpty(categories) && !categories.Equals("Все"))
            {
                Blogs_d.Clear();
                for (int i_1 = 0; i_1 < blogs.Count(); i_1++)
                {
                    if (blogs.ToList()[i_1].Tag == categories)
                    {
                        if (!String.IsNullOrEmpty(tag) && !tag.Equals("Все"))
                        {

                            for (int i = 0; i < Blogs.Count(); i++)
                            {
                                int count_tags = Blogs.ToList()[i].Tags.Count();
                                for (int j = 0; j < count_tags; j++)
                                {
                                    if (Blogs.ToList()[i].Tags.ToList()[j].Name == tag && Blogs.ToList()[i].Tag == categories)
                                    {
                                        string name_tag = Blogs.ToList()[i].Name;
                                        if(!Blogs_d.Contains(Blogs.ToList()[i]))
                                        Blogs_d.Add(Blogs.ToList()[i]);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            BlogsListViewModel plvm = new BlogsListViewModel
            {
                Blogs = Blogs_d.ToList(),
                Categories = Categories,
                Tags = tags
            };

            string result = "Вы не авторизованы";
            string name_user = "";
            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
                name_user = User.Identity.Name;
            }


            bool IsAdmin = HttpContext.User.IsInRole("admin");
            if (User.Identity.IsAuthenticated)
            {
                if (!IsAdmin)
                    ViewBag.Result = result + ", вы не Администратор и не можете Удалять | Редактировать Блоги.";
                else
                    ViewBag.Result = result + ", вы Администратор.";
            }
            else
            {
                ViewBag.Result = result;
            }

            ViewData["username"] = name_user;
            return View("Index", plvm);
        }

        public ActionResult CreateTag(int id)
        {
            ViewBag.Tags = db.Tags.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTag(Tag tag, int id, string name)
        {
            if (name == "")
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("EditBlog", new { id });
            }
            else
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Create", new { name });
            }
        }

        [HttpGet]
        public ActionResult Create(string name)
        {
            if (name == null)
            {
                return HttpNotFound();
            }
            SelectList tags = new SelectList(db.Tags);
            ViewBag.Tags = db.Tags.ToList();
            ViewBag.Categories = Categories;
            Models.Blog blog = new Models.Blog();
            return View(blog);
        }

        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public ActionResult Create(Models.Blog blog, int[] selectedTags, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                Models.Blog newBlog = new Models.Blog();
                newBlog.Name = blog.Name;
                newBlog.MainText = blog.MainText;
                newBlog.Tag = blog.Tag;

                if (Image != null)
                {
                    newBlog.ImageMimeType = Image.ContentType;
                    newBlog.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(newBlog.ImageData, 0, Image.ContentLength);
                }

                newBlog.Tags.Clear();
                if (selectedTags != null)
                {
                    foreach (var c in db.Tags.Where(co => selectedTags.Contains(co.Id)))
                    {
                        newBlog.Tags.Add(c);
                    }
                }
                newBlog.AuthorId = 2;
                Author a = new Author();
                a.Name = User.Identity.Name;
                a.Id = 2;
                newBlog.Author = a;
                db.Blogs.Add(newBlog);
                db.SaveChanges();

            }
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult EditBlog(int? id)
        {

            if (id == null)
            {
                return HttpNotFound();
            }
            Models.Blog blog = db.Blogs.Find(id);
            if (blog != null)
            {
                ViewBag.Tags = db.Tags.ToList();
                ViewBag.Categories = Categories;
                return View(blog);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditBlog(Models.Blog blog, int[] selectedTags, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                Models.Blog newBlog = db.Blogs.Find(blog.Id);
                newBlog.Name = blog.Name;
                newBlog.MainText = blog.MainText;
                newBlog.Tag = blog.Tag;

                if (Image != null)
                {
                    newBlog.ImageMimeType = Image.ContentType;
                    newBlog.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(newBlog.ImageData, 0, Image.ContentLength);
                }

                newBlog.Tags.Clear();
                if (selectedTags != null)
                {
                    foreach (var c in db.Tags.Where(co => selectedTags.Contains(co.Id)))
                    {
                        newBlog.Tags.Add(c);
                    }
                }
                db.Entry(newBlog).State = EntityState.Modified;
                newBlog.AuthorId = 2;
                Author a = new Author();
                a.Name = User.Identity.Name;
                a.Id = 2;
                newBlog.Author = a;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Models.Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
                db.Blogs.Remove(blog);
                db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Read(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Models.Blog blog = db.Blogs.Find(id);
            if (blog != null)
            {
                SelectList tags = new SelectList(db.Tags);
                ViewBag.Tags = tags;
                return View(blog);
            }
            return HttpNotFound();
        }

        public ActionResult AuthorDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Author author = db.Authors.Include(t => t.Blogs).FirstOrDefault(t => t.Id == id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (BlogContext db = new BlogContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Name == model.Name && u.Password == model.Password);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (BlogContext db = new BlogContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Name == model.Name);
                }
                if (user == null)
                {
                    using (BlogContext db = new BlogContext())
                    {
                        Author author = new Author();
                        author.Name = model.Name;
                        db.Authors.Add(author);
                        Role user_r = db.Roles.Where(u => u.Id == 2).FirstOrDefault();
                        db.Users.Add(new User { Name = model.Name, Password = model.Password, Author = author, Role = user_r });
                        db.SaveChanges();
                        user = db.Users.Where(u => u.Name == model.Name && u.Password == model.Password).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);

                        return RedirectToAction("Index", "Blog");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Blog");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}