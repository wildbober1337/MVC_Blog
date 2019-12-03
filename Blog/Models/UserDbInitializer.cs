using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class UserDbInitializer : DropCreateDatabaseAlways<BlogContext>
    {
        protected override void Seed(BlogContext db)
        {
            string[] maintext = { "ASP.NET (Active Server Pages для .NET) — платформа разработки веб-приложений, в состав которой входит: веб-сервисы, программная инфраструктура, модель программирования[1], от компании Майкрософт. ASP.NET входит в состав платформы .NET Framework[2] и является развитием более старой технологии Microsoft ASP. ASP.NET внешне во многом сохраняет схожесть с более старой технологией ASP, что позволяет разработчикам относительно легко перейти на ASP.NET. В то же время внутреннее устройство ASP.NET существенно отличается от ASP, поскольку она основана на платформе .NET и, следовательно, использует все новые возможности, предоставляемые этой платформой.",
                "Java[прим. 1] — строго типизированный объектно-ориентированный язык программирования, разработанный компанией Sun Microsystems (в последующем приобретённой компанией Oracle). Разработка ведётся сообществом, организованным через Java Community Process, язык и основные реализующие его технологии распространяются по лицензии GPL. Права на торговую марку принадлежат корпорации Oracle. Приложения Java обычно транслируются в специальный байт-код, поэтому они могут работать на любой компьютерной архитектуре с помощью виртуальной Java-машины. Дата официального выпуска — 23 мая 1995 года. На 2019 год Java — один из самых популярных языков программирования[2][3].",
                "Unity — межплатформенная среда разработки компьютерных игр[1]. Unity позволяет создавать приложения, работающие под более чем 20 различными операционными системами, включающими персональные компьютеры, игровые консоли, мобильные устройства, интернет-приложения и другие[2]. Выпуск Unity состоялся в 2005 году и с того времени идёт постоянное развитие.Основными преимуществами Unity являются наличие визуальной среды разработки, межплатформенной поддержки и модульной системы компонентов. К недостаткам относят появление сложностей при работе с многокомпонентными схемами и затруднения при подключении внешних библиотек[⇨].На Unity написаны тысячи игр, приложений и симуляций, которые охватывают множество платформ и жанров. При этом Unity используется как крупными разработчиками, так и независимыми студиями[⇨].",
                "PHP (/pi:.eɪtʃ.pi:/ англ. PHP: Hypertext Preprocessor — «PHP: препроцессор гипертекста»; первоначально Personal Home Page Tools[11] — «Инструменты для создания персональных веб-страниц») — скриптовый язык[12] общего назначения, интенсивно применяемый для разработки веб-приложений. В настоящее время поддерживается подавляющим большинством хостинг-провайдеров и является одним из лидеров среди языков, применяющихся для создания динамических веб-сайтов[13]. Язык и его интерпретатор (Zend Engine) разрабатываются группой энтузиастов в рамках проекта с открытым кодом[14]. Проект распространяется под собственной лицензией, несовместимой с GNU GPL.","health food" };

            Blog blog = new Blog { Name = "Asp", MainText = maintext[0], Tag = "Программирование" };
            Blog blog1 = new Blog { Name = "Java", MainText = maintext[1], Tag = "Программирование" };
            Blog blog2 = new Blog { Name = "Unity3d", MainText = maintext[2], Tag = "Программирование" };
            Blog blog3 = new Blog { Name = "PHP", MainText = maintext[3], Tag = "Программирование" };

            Blog blog4 = new Blog { Name = "Правильный образ жизни", MainText = maintext[4], Tag = "Культура и Здоровье" };


            Tag c1 = new Tag
            {
                Id = 1,
                Name = "Фреймворк",
                Blogs = new List<Blog>() { blog, blog2 }
            };
            Tag c2 = new Tag
            {
                Id = 2,
                Name = "Игровой Движок",
                Blogs = new List<Blog>() { blog2 }
            };
            Tag c3 = new Tag
            {
                Id = 3,
                Name = "Язык программирования",
                Blogs = new List<Blog>() {  blog3, blog1, blog, blog2 }
            };
            Tag c4 = new Tag
            {
                Id = 4,
                Name = "Полезная еда",
                Blogs = new List<Blog>() { blog4 }
            };

            db.Tags.Add(c1);
            db.Tags.Add(c2);
            db.Tags.Add(c3);
            db.Tags.Add(c4);

            Role admin = new Role { Name = "admin" };
            Role user = new Role { Name = "user" };

            db.Roles.Add(admin);
            db.Roles.Add(user);
            Author author = new Author { Name = "admin" };

            author.Blogs.Add(blog);
            author.Blogs.Add(blog1);
            author.Blogs.Add(blog2);
            author.Blogs.Add(blog3);
            author.Blogs.Add(blog4);

            db.Users.Add(new User
            {
                Name = "admin",
                Password = "admin",
                Role = admin,
                Author = author        
            });



            base.Seed(db);
        }
    }
}