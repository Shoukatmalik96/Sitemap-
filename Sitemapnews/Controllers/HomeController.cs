using Sitemapnews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Sitemapnews.Helpers.RssFeedHelper;

namespace Sitemapnews.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sitemap()
        {
            var siteMapList = new List<SiteMapEntity>();

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/",
                changefreq = "hourly",
            });
            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/mainnewslist/0/0/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/list/101/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/sportslist/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/list/103/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/list/104/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/football/listleaguevideos/0/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/article/allvideos/1",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/ContactUs",
                changefreq = "hourly",
            });

            siteMapList.Add(new SiteMapEntity()
            {
                loc = "http://akhbaar24.argaam.com/home/aboutus",
                changefreq = "hourly",
                lastmod = "2012-06-03T14:00:27+00:00"
            }); ;

            Sitemapper sitemapper = new Sitemapper();

            foreach (var siteMapItem in siteMapList)
            {
                sitemapper.WriteItem(siteMapItem.loc, siteMapItem.changefreq, siteMapItem.lastmod);
            }

            return Content(sitemapper.ToString, "text/xml");
        }
    }
}