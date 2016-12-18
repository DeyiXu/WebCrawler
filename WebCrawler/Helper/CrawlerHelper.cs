using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebCrawler.Model;

namespace WebCrawler.Helper
{
    public class CrawlerHelper
    {
        public string HtmlCode { get; set; }
        public CrawlerHelper(string htmlCode)
        {
            HtmlCode = htmlCode;
        }
        /// <summary>
        /// 获取当前页面的URL
        /// </summary>
        public string[] GetLinks
        {
            get
            {
                //const string PATTERN = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                const string PATTERN = @"(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?";
                Regex r = new Regex(PATTERN, RegexOptions.IgnoreCase);
                MatchCollection m = r.Matches(HtmlCode);
                List<string> links = new List<string>();

                for (int i = 0; i < m.Count; i++)
                {
                    string url = m[i].ToString();
                    if (!string.IsNullOrEmpty(url) && UrlAvailable(url))
                    {
                        links.Add(url);
                    }
                }
                return links.ToArray();
            }
        }
        /// <summary>
        /// 获取当前页面的内容
        /// </summary>
        public string GetContent
        {
            get
            {
                HtmlDocument hd = new HtmlDocument();
                hd.LoadHtml(HtmlCode);
                HtmlNode hn = hd.DocumentNode.SelectSingleNode("//*[@id=\"cnblogs_post_body\"]");
                if (hn != null)
                {
                    return hn.InnerHtml;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取当前页面文章内容
        /// </summary>
        public Article GetArticle
        {
            get
            {
                Article article = new Article();

                HtmlDocument hd = new HtmlDocument();
                hd.LoadHtml(HtmlCode);
                HtmlNode hnTitle = hd.DocumentNode.SelectSingleNode("//*[@id=\"cb_post_title_url\"]");
                if (hnTitle != null)
                {
                    article.Title = hnTitle.InnerText;
                }
                HtmlNode hnKeywords = hd.DocumentNode.SelectSingleNode("//meta[@name=\"keywords\"]");
                if (hnKeywords != null)
                {
                    article.Keywords = hnKeywords.Attributes["content"].Value;
                }
                HtmlNode hnDescription = hd.DocumentNode.SelectSingleNode("//meta[@name=\"description\"]");
                if (hnKeywords != null)
                {
                    article.Description = hnDescription.Attributes["content"].Value;
                }
                HtmlNode hnContent = hd.DocumentNode.SelectSingleNode("//*[@id=\"cnblogs_post_body\"]");
                if (hnContent != null)
                {
                    article.Content = hnContent.InnerHtml;
                }
                HtmlNode hnCreateTime = hd.DocumentNode.SelectSingleNode("//*[@id=\"post-date\"]");
                if (hnCreateTime != null)
                {
                    try
                    {
                        article.CreateTime = DateTime.Parse(hnCreateTime.InnerText);
                    }
                    catch
                    {
                        article.CreateTime = DateTime.MinValue;
                    }
                }

                article.CreateTime = DateTime.Now;
                article.Tags = "";

                return article;
            }
        }


        /// <summary>
        /// 检测链接是否为有效连接
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool UrlAvailable(string url)
        {
            url = url.ToLower();
            string[] suffixs = { ".mp4", ".xml", ".svg", ".jpg", ".jpeg", ".ico", ".gif", ".png", ".css", ".js" };
            foreach (string suffix in suffixs)
            {
                if (url.Contains(suffix))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
