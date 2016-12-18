using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DAL;
using WebCrawler.Model;

namespace WebCrawler.BLL
{
    public class LinkBLL
    {
        public bool SelectExistByUrl(string url)
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.SelectExistByUrl(url);
        }
        public void AddUrl(string url, int depth)
        {
            LinkDAL dalLink = new LinkDAL();

            if (!dalLink.SelectExistByUrl(url))
            {
                Link link = new Link();
                link.Url = url;
                link.Depth = depth;
                link.Status = Link.StatusAttribute.IsUse;
                link.CreateTime = DateTime.Now;
                link.UpdateTime = DateTime.Now;
                link.IsDownload = false;
                int insertId = dalLink.Insert(link);
                link.Id = insertId;
            }
        }
        public void AddUrls(string[] urls, int depth, string baseUrl)
        {
            LinkDAL dalLink = new LinkDAL();
            foreach (string url in urls)
            {
                string cleanUrl = url.Trim();
                int end = cleanUrl.IndexOf(' ');
                if (end > 0)
                {
                    cleanUrl = cleanUrl.Substring(0, end);
                }

                //检测是否是当前网站的链接
                if (cleanUrl.Contains(baseUrl) && !SelectExistByUrl(cleanUrl))
                {
                    Link link = new Link();
                    link.Url = cleanUrl;
                    link.Depth = depth;
                    link.CreateTime = DateTime.Now;
                    link.UpdateTime = DateTime.Now;
                    link.Status = Link.StatusAttribute.IsUse;

                    int insertId = dalLink.Insert(link);
                    link.Id = insertId;
                }
                else
                {
                    // 外链
                }
            }
        }
        public bool UpdateStatusByNotUse(string url)
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.UpdateStatusByUrl(url, Link.StatusAttribute.NotUse);
        }
        public bool UpdateStatusByIsUse(string url)
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.UpdateStatusByUrl(url, Link.StatusAttribute.IsUse);
        }
        public int SelectCountByStatusIsUse()
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.SelectCountByStatus(Link.StatusAttribute.IsUse);
        }
        public int SelectCountByStatusNotUse()
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.SelectCountByStatus(Link.StatusAttribute.NotUse);
        }
        public List<Link> SelectByStatus(int top, Link.StatusAttribute status)
        {
            List<Link> links = new List<Link>();
            LinkDAL dalLink = new LinkDAL();
            links = dalLink.SelectByStatus(top, status);
            return links;
        }
        /// <summary>
        /// 可以下载的URL
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Link> SelectByIsUseDownload(int top, string suffix = "")
        {
            List<Link> links = new List<Link>();
            LinkDAL dalLink = new LinkDAL();
            if (suffix == string.Empty)
            {
                links = dalLink.SelectByIsDownload(top, false);
            }
            else
            {
                links = dalLink.SelectByIsDownload(top, false, suffix);
            }
            return links;
        }
        /// <summary>
        /// 不可以下载的URL
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Link> SelectByNotUseDownload(int top)
        {
            List<Link> links = new List<Link>();
            LinkDAL dalLink = new LinkDAL();
            links = dalLink.SelectByIsDownload(top, true);
            return links;
        }
        public bool UpdateIsDownloadById(int id)
        {
            LinkDAL dalLink = new LinkDAL();
            return dalLink.UpdateIsDownloadById(id, true);
        }
    }
}
