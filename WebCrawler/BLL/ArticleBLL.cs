using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Model;
using WebCrawler.DAL;

namespace WebCrawler.BLL
{
    public class ArticleBLL
    {
        public int Insert(Article art)
        {
            ArticleDAL dalArticle = new ArticleDAL();
            int insertId = dalArticle.Insert(art);
            return insertId;
        }
        public int InsertByLinkId(Article art, int linkId)
        {
            ArticleDAL dalArticle = new ArticleDAL();
            int insertId = dalArticle.Insert(art);
            if (insertId > 0)
            {
                LinkDAL dalLink = new LinkDAL();
                dalLink.UpdateIsDownloadById(linkId, true);
            }
            return insertId;
        }
    }
}
