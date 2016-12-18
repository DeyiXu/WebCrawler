using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Model;

namespace WebCrawler.DAL
{
    public class ArticleDAL : DALBase
    {
        public int Insert(Article art)
        {
            string sql = "INSERT INTO [dbo].[Articles] ([ArticleUrl],[Title],[Keywords],[Description],[Content],[Tags],[CreateTime]) VALUES (@ArticleUrl,@Title,@Keywords,@Description,@Content,@Tags,@CreateTime);SELECT @InsertId = @@Identity;";
            SqlParameter[] parameters ={
                new SqlParameter("@ArticleUrl",art.ArticleUrl),
                new SqlParameter("@Title",art.Title),
                new SqlParameter("@Keywords",art.Keywords),
                new SqlParameter("@Description",art.Description),
                new SqlParameter("@Content",art.Content),
                new SqlParameter("@Tags",art.Tags),
                new SqlParameter("@CreateTime",art.CreateTime),
                new SqlParameter(INSERTID,SqlDbType.Int)
            };
            parameters[parameters.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, parameters);
            int insertId = int.Parse(parameters[parameters.Length - 1].Value.ToString());
            return insertId;
        }
    }
}
