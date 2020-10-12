using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitemapnews.Models
{
    public class SiteMapEntity
    {
        public string loc { get; set; }
        public string changefreq { get; set; }
        public string lastmod { get; set; }
    }
}