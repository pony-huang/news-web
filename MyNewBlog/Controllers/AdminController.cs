﻿using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MyNewBlog.Models;
using PagedList;
using Microsoft.AspNetCore.Mvc;

namespace MyNewBlog.Controllers
{
    public class AdminController : Controller
    {
        private NewsInformationEntities db = new NewsInformationEntities();


        //public ViewResult Articles(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    var articles = from s in db.Article
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        articles = articles.Where(s => s.title.Contains(searchString)
        //                               || s.author.Contains(searchString));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            articles = articles.OrderByDescending(s => s.author);
        //            break;
        //        case "Date":
        //            articles = articles.OrderBy(s => s.date);
        //            break;
        //        case "date_desc":
        //            articles = articles.OrderByDescending(s => s.categoryId);
        //            break;
        //        default:  // Name ascending 
        //            articles = articles.OrderBy(s => s.date);
        //            break;
        //    }

        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    String[] cateNames = {"","时政新闻","国际新闻","财经新闻","体育新闻"
        //            ,"教育新闻","游戏新闻","时尚新闻","科技新闻","传媒新闻" };
        //    ViewBag.cateNames = cateNames;
        //    return View(articles.ToPagedList(pageNumber, pageSize));
        //}

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }


        //GET: Cates 模态框获取分类
        [HttpGet]
        public JsonResult GetCates()
        {
            var cates = from c in db.Category
                        select c;

            return Json(cates,JsonRequestBehavior.AllowGet);
        }


        //GET: Cates 模态框获取分类
        [HttpGet]
        public JsonResult GetArticleInfo(int id)
        {
            var article = from a in db.Article
                           where a.id == id
                           select a;
            var json = new JsonResult();
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.Data = article.ToList();
            return json;
        }

        //GET: Dashboard/UserDetails
        public ActionResult UserDetails(int? page)
        {
            int pageSize = 10;

            if (page == 0)
            {
                page = 1;
            }
            int pageNumber = page ?? 1;

            return View(db.User.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Users(int ? page)
        {
            int pageSize = 10;

            if (page == 0)
            {
                page = 1;
            }
            int pageNumber = page ?? 1;
            var Users = db.User.ToList().ToPagedList(pageNumber, pageSize);
            ViewBag.Users = Users;
            return View();
        }


        //GET: Dashboard/Articles
        public ActionResult Articles(int? page)
        {
            int pageSize = 20;

            if (page == 0)
            {
                page = 1;
            }
            int pageNumber = page ?? 1;

            var categories = from c in db.Category
                             select c;
            String[] cateNames = {"","时政新闻","国际新闻","财经新闻","体育新闻"
                    ,"教育新闻","游戏新闻","时尚新闻","科技新闻","传媒新闻" };
            ViewBag.cateNames = cateNames;
            return View(db.Article.ToList().ToPagedList(pageNumber, pageSize));
        }

        //GET: Dashboard/Articles
        public ActionResult Links(int? page)
        {
            int pageSize = 20;

            if (page == 0)
            {
                page = 1;
            }
            int pageNumber = page ?? 1;
            var links = db.Link.ToList().ToPagedList(pageNumber,pageSize);
            ViewBag.Links = links;
            return View();
        }



        // POST: Dashboard/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article)
        {
            //int id,string title,string link,DateTime date,string description,string author

            System.Diagnostics.Debug.Write(article.author);
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
            }
            return RedirectToAction("/Articles");
        }



        //添加文章
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Article article)
        {
            //int id,string title,string link,DateTime date,string description,string author

            if (ModelState.IsValid)
            {
                db.Article.Add(article);
                db.SaveChanges();
            }
            return RedirectToAction("Articles");
        }





        [HttpPost]
        public JsonResult Delete(string Ids)
        {
            System.Diagnostics.Debug.Write(Ids);
            if (Ids == null || Ids == "")
            {
                return Json("null value");
            }

            if (!Ids.Contains("-"))
            {
                int id = Convert.ToInt32(Ids);
                Article article = db.Article.Find(id);
                if (article == null)
                {
                    return Json("not exit ids");
                }
                db.Article.Remove(article);
                db.SaveChanges();
            }
            else
            {
                string[] idstr = Ids.Split('-');
                foreach (string str in idstr)
                {
                    int id = Convert.ToInt32(str);
                    Article article = db.Article.Find(id);
                    db.Article.Remove(article);
                    db.SaveChanges();
                }
            }
            return Json(true);
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