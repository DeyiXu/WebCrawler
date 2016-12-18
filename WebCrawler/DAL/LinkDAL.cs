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
    public class LinkDAL : DALBase
    {
        private const string SELECT = "SELECT [Id],[Url],[Depth],[CreateTime],[UpdateTime],[Status],[IsDownload] FROM [dbo].[Links] ";
        private const string SELECT_COLUMNS = " [Id],[Url],[Depth],[CreateTime],[UpdateTime],[Status],[IsDownload] ";
        public bool SelectExistByUrl(string url)
        {
            string sql = "SELECT COUNT('x') FROM [dbo].[Links] WHERE [Url] = @Url" + SELECT_COLLATE;
            SqlParameter parameter = new SqlParameter("@Url", url);
            int count = (int)SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sql, parameter);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Link> SelectByStatus(int top, Link.StatusAttribute status)
        {
            string sqlStr = "SELECT TOP (@top) " + SELECT_COLUMNS + "FROM [dbo].[Links] WHERE [Status] = @Status;";
            SqlParameter[] parameters ={
                new SqlParameter("@top",top),
                new SqlParameter("@Status",status)
            };
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters));
            return links;
        }
        public List<Link> SelectByStatus(Link.StatusAttribute status)
        {
            string sqlStr = SELECT + "WHERE [Status] = @Status;";
            SqlParameter[] parameters ={
                new SqlParameter("@Status",status)
            };
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlStr));
            return links;
        }
        public List<Link> SelectByIsDownload(int top, bool isDownload, string suffix)
        {
            string sqlStr = "SELECT TOP (@top) " + SELECT_COLUMNS + "FROM [dbo].[Links] WHERE [IsDownload] = @IsDownload AND [Url] LIKE @suffix;";
            SqlParameter[] parameters ={
                new SqlParameter("@top",top),
                new SqlParameter("@IsDownload",isDownload),
                new SqlParameter("@suffix","%"+suffix)
            };
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters));
            return links;
        }
        public List<Link> SelectByIsDownload(int top, bool isDownload)
        {
            string sqlStr = "SELECT TOP (@top) " + SELECT_COLUMNS + "FROM [dbo].[Links] WHERE [IsDownload] = @IsDownload;";
            SqlParameter[] parameters ={
                new SqlParameter("@top",top),
                new SqlParameter("@IsDownload",isDownload)
            };
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters));
            return links;
        }
        public List<Link> SelectByIsDownload(bool isDownload)
        {
            string sqlStr = SELECT + "WHERE [IsDownload] = @IsDownload;";
            SqlParameter[] parameters ={
                new SqlParameter("@IsDownload",isDownload)
            };
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlStr));
            return links;
        }
        public List<Link> Select()
        {
            List<Link> links = Readers(SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, SELECT));
            return links;
        }
        public int Insert(Link l)
        {
            string sql = "INSERT INTO [dbo].[Links]([Url],[Depth],[CreateTime],[UpdateTime],[Status],[IsDownload]) VALUES (@Url,@Depth,@CreateTime,@UpdateTime,@Status,@IsDownload);SELECT @InsertId = @@Identity;";
            SqlParameter[] parameters ={
                new SqlParameter("@Url",l.Url),
                new SqlParameter("@Depth",l.Depth),
                new SqlParameter("@CreateTime",l.CreateTime),
                new SqlParameter("@UpdateTime",l.UpdateTime),
                new SqlParameter("@Status", l.Status),
                new SqlParameter("@IsDownload", l.IsDownload),
                new SqlParameter(INSERTID,SqlDbType.Int)
            };
            parameters[parameters.Length - 1].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sql, parameters);
            int insertId = int.Parse(parameters[parameters.Length - 1].Value.ToString());
            return insertId;
        }
        public bool UpdateStatusById(int id, Link.StatusAttribute status)
        {
            string sqlStr = "UPDATE [dbo].[Links] SET [UpdateTime] = @UpdateTime,[Status] = @Status WHERE Id =@id";
            SqlParameter[] parameters ={
                new SqlParameter ("@UpdateTime",DateTime.Now),
                new SqlParameter ("@Status",status),
                new SqlParameter ("@id",id)
            };
            int row = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters);
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateIsDownloadById(int id, bool IsDownload)
        {
            string sqlStr = "UPDATE [dbo].[Links] SET [IsDownload] = @IsDownload WHERE Id =@id";
            SqlParameter[] parameters ={
                new SqlParameter ("@IsDownload",IsDownload),
                new SqlParameter ("@id",id)
            };
            int row = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters);
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateStatusByUrl(string url, Link.StatusAttribute status)
        {
            string sqlStr = "UPDATE [dbo].[Links] SET [UpdateTime] = @UpdateTime,[Status] = @Status WHERE Url =@Url";
            SqlParameter[] parameters ={
                new SqlParameter ("@UpdateTime",DateTime.Now),
                new SqlParameter ("@Status",status),
                new SqlParameter ("@Url",url),
            };
            int row = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters);
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int SelectCountByStatus(Link.StatusAttribute status)
        {
            string sqlStr = "SELECT COUNT('x') FROM [dbo].[Links] WHERE [Status] = @Status;";
            SqlParameter[] parameters ={
                new SqlParameter("@Status",status)
            };
            int count = (int)SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, sqlStr, parameters);
            return count;
        }

        private Link Reader(SqlDataReader reader)
        {
            Link link = null;
            if (reader.Read())
            {
                link = new Link();
                link.Id = reader["Id"] == DBNull.Value ? 0 : int.Parse(reader["Id"].ToString());
                link.Url = reader["Url"].ToString();
                link.Depth = reader["Depth"] == DBNull.Value ? 0 : int.Parse(reader["Depth"].ToString());
                link.CreateTime = reader["CreateTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(reader["CreateTime"].ToString());
                link.UpdateTime = reader["UpdateTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(reader["UpdateTime"].ToString());
                link.Status = reader["Status"] == DBNull.Value ? Link.StatusAttribute.NotUse : (Link.StatusAttribute)((short)reader["Status"]);
            }
            reader.Close();
            return link;
        }
        private List<Link> Readers(SqlDataReader reader)
        {
            List<Link> links = new List<Link>();
            Link link = null;
            while (reader.Read())
            {
                link = new Link();
                link.Id = reader["Id"] == DBNull.Value ? 0 : int.Parse(reader["Id"].ToString());
                link.Url = reader["Url"].ToString();
                link.Depth = reader["Depth"] == DBNull.Value ? 0 : int.Parse(reader["Depth"].ToString());
                link.CreateTime = reader["CreateTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(reader["CreateTime"].ToString());
                link.UpdateTime = reader["UpdateTime"] == DBNull.Value ? DateTime.MinValue : DateTime.Parse(reader["UpdateTime"].ToString());
                link.Status = reader["Status"] == DBNull.Value ? Link.StatusAttribute.NotUse : (Link.StatusAttribute)((short)reader["Status"]);
                links.Add(link);
            }
            reader.Close();
            return links;
        }
    }
}
