using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WebCrawler.Helper;
using WebCrawler.Model;
using WebCrawler.BLL;

namespace WebCrawler
{
    class Program
    {
        #region 属性
        static List<Link> linkList = new List<Link>();
        static string _baseDirPath = AppDomain.CurrentDomain.BaseDirectory + "articleList\\";
        static string _articleSuffix = ".html";
        static string _baseUrl = "http://www.cnblogs.com";
        static bool _isDownload = true;
        /// <summary>
        /// 下载路径
        /// </summary>
        public static string BaseDirPath
        {
            get
            {
                return _baseDirPath;
            }

            set
            {
                _baseDirPath = value;
            }
        }
        /// <summary>
        /// 文章后缀
        /// </summary>
        public static string ArticleSuffix
        {
            get
            {
                return _articleSuffix;
            }

            set
            {
                _articleSuffix = value;
            }
        }
        /// <summary>
        /// 根域名
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                return _baseUrl;
            }

            set
            {
                _baseUrl = value;
            }
        }
        /// <summary>
        /// 是否下载
        /// </summary>
        public static bool IsDownload
        {
            get
            {
                return _isDownload;
            }

            set
            {
                _isDownload = value;
            }
        }
        #endregion

        static void Main(string[] args)
        {
            ConsoleInitial();
            LinkBLL bllLink = new LinkBLL();
            ArticleBLL bllArticle = new ArticleBLL();
            linkList = bllLink.SelectByStatus(100, Link.StatusAttribute.IsUse);

            if (linkList.Count <= 0)
            {
                Link link = new Link();
                link.Url = BaseUrl;
                link.Depth = 0;
                bllLink.AddUrl(BaseUrl, 0);
                linkList.Add(link);
            }
            int isUseLinkIndex = linkList.Count - 1;
            while (linkList.Count > 0)
            {
                Link link = linkList[isUseLinkIndex];

                //添加加载记录,修改成不能用（下次不查询）
                //Loaded.Add(url, depth);
                bllLink.UpdateStatusByNotUse(link.Url);
                HttpHelper.HttpItem httpItem = new HttpHelper.HttpItem();
                httpItem.URL = link.Url;
                httpItem.Method = "GET";
                httpItem.Encoding = Encoding.UTF8;
                string html = HttpHelper.GetHtml(httpItem);

                CrawlerHelper crawlerHelper = new CrawlerHelper(html);
                string[] links = crawlerHelper.GetLinks;

                AddUrls(links, link.Depth + 1, BaseUrl);

                Console.WriteLine(string.Format("需加载{0},已加载{1}", bllLink.SelectCountByStatusIsUse(), bllLink.SelectCountByStatusNotUse()));
                //删除 当前需要加载的
                //Unload.Remove(url);
                isUseLinkIndex--;
                if (isUseLinkIndex <= -1)
                {
                    linkList = bllLink.SelectByStatus(100, Link.StatusAttribute.IsUse);
                    isUseLinkIndex = linkList.Count - 1;
                }
            }


            //加载需要下载的链接
            linkList = bllLink.SelectByIsUseDownload(100, ArticleSuffix);
            int isDownloadIndex = linkList.Count - 1;
            while (linkList.Count > 0)
            {
                Link link = linkList[isDownloadIndex];
                Console.WriteLine("----{0}=={1}-----", isDownloadIndex, link.Id);

                HttpHelper.HttpItem httpItem = new HttpHelper.HttpItem();
                httpItem.URL = link.Url;
                httpItem.Method = "GET";
                httpItem.Encoding = Encoding.UTF8;
                string html = HttpHelper.GetHtml(httpItem);

                CrawlerHelper crawlerHelper = new CrawlerHelper(html);
                Article article = crawlerHelper.GetArticle;
                if (article.Title != string.Empty && article.Content != string.Empty)
                {
                    article.ArticleUrl = link.Url;
                    int artInsertId = bllArticle.InsertByLinkId(article, link.Id);
                    if (artInsertId > 0)
                    {
                        Console.WriteLine("添加成功-----{0}", article.Title);
                    }
                }
                else
                {
                    bllLink.UpdateIsDownloadById(link.Id);
                }
                isDownloadIndex--;
                if (isDownloadIndex <= -1)
                {
                    linkList = bllLink.SelectByIsUseDownload(100, ArticleSuffix);
                    isDownloadIndex = linkList.Count - 1;
                }
            }
            Console.ReadKey();


            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }

        #region 方法
        /// <summary>
        /// 初始化控制台
        /// </summary>
        static void ConsoleInitial()
        {
            Console.Title = "网络爬虫　By德意洋洋 http://www.xudeyi.com";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.Write("请输入网址(默认:http://www.cnblogs.com):");
            string url = Convert.ToString(Console.ReadLine());
            if (!string.IsNullOrEmpty(url))
            {
                BaseUrl = url;
            }
            Console.Write("请输入文章后缀(默认.html):");
            string suffix = Convert.ToString(Console.ReadLine());
            if (!string.IsNullOrEmpty(suffix))
            {
                ArticleSuffix = suffix;
            }
            Console.Write("是否下载文件：(1/0,默认1):");
            string bl = Convert.ToString(Console.ReadLine());
            if (!string.IsNullOrEmpty(bl))
            {
                IsDownload = (bl == "1" ? true : false);
            }
        }
        /// <summary>
        /// 添加链接
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="depth"></param>
        /// <param name="baseUrl"></param>
        /// <param name="unload"></param>
        /// <param name="loaded"></param>
        private static void AddUrls(string[] urls, int depth, string baseUrl)
        {
            if (depth >= 10)
            {
                return;
            }
            LinkBLL bllLink = new LinkBLL();
            bllLink.AddUrls(urls, depth, baseUrl);

        }
        #endregion
    }
}
