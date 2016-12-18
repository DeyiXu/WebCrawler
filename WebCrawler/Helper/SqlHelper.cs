//===============================================================================
// This file is based on the Microsoft Data Access Application Block for .NET
// For more information please go to 
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebCrawler.DAL
{
    /// <summary>
    /// ִ��Sql �����ͨ�÷���
    /// </summary>
    public abstract class SqlHelper
    {
        //Database connection strings
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["WebCrawler"].ConnectionString;

        #region ExecuteNonQuery
        /// <summary>
        /// ִ��sql����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>��Ӱ�������</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, commandType, conn, commandText, commandParameters);
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }

        /// <summary>
        /// ִ��MySQL Server�洢����
        /// ע�⣺����ִ����out �����Ĵ洢����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">�������</param>
        /// <returns>��Ӱ�������</returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, conn, spName, parameterValues);
                int val = cmd.ExecuteNonQuery();

                return val;
            }
        }
        #endregion

        #region ExecuteObject
        /// <summary>
        ///  ִ��sql����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>T ����</returns>
        public static T ExecuteObject<T>(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (SqlDataReader reader = ExecuteReader(connectionString, commandType, commandText, commandParameters))
                {
                    T t = System.Activator.CreateInstance<T>();
                    Type obj = t.GetType();
                    if (reader.Read())
                    {
                        // ѭ���ֶ�  
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object tempValue = null;
                            if (reader.IsDBNull(i))
                            {
                                string typeFullName = obj.GetProperty(reader.GetName(i)).PropertyType.FullName;
                                tempValue = GetDBNullValue(typeFullName);
                            }
                            else
                            {
                                tempValue = reader.GetValue(i);
                            }
                            obj.GetProperty(reader.GetName(i)).SetValue(t, tempValue, null);
                        }
                        return t;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  ִ��sql����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>List<T> ����</returns>
        public static List<T> ExecuteObjects<T>(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                using (SqlDataReader reader = ExecuteReader(connectionString, commandType, commandText, commandParameters))
                {
                    List<T> list = new List<T>();
                    while (reader.Read())
                    {
                        T t = System.Activator.CreateInstance<T>();
                        Type obj = t.GetType();
                        // ѭ���ֶ�  
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object tempValue = null;
                            if (reader.IsDBNull(i))
                            {
                                string typeFullName = obj.GetProperty(reader.GetName(i)).PropertyType.FullName;
                                tempValue = GetDBNullValue(typeFullName);
                            }
                            else
                            {
                                tempValue = reader.GetValue(i);
                            }
                            obj.GetProperty(reader.GetName(i)).SetValue(t, tempValue, null);
                        }
                        list.Add(t);
                    }
                    return list;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        ///  ִ��sql����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>SqlDataReader ����</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, commandType, conn, commandText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// ִ��MySQL Server�洢����
        /// ע�⣺����ִ����out �����Ĵ洢����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">�������</param>
        /// <returns>��Ӱ�������</returns>
        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, conn, spName, parameterValues);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }

        }
        #endregion

        #region ExecuteDataset

        /// <summary>
        /// ִ��MySQL Server�洢����
        /// ע�⣺����ִ����out �����Ĵ洢����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">�������</param>
        /// <returns>DataSet����</returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, conn, spName, parameterValues);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    da.Fill(ds);

                    return ds;
                }
            }
        }


        /// <summary>
        /// ִ��Sql ����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>DataSet ����</returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                SqlCommand cmd = new SqlCommand();

                PrepareCommand(cmd, commandType, conn, commandText, commandParameters);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();

                    da.Fill(ds);

                    return ds;
                }
            }
        }

        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ִ��Sql ����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">sql���/������sql���/�洢������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>ִ�н������</returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, commandType, conn, commandText, commandParameters);
                object val = cmd.ExecuteScalar();

                return val;
            }
        }

        /// <summary>
        /// ִ��MySQL Server�洢����
        /// ע�⣺����ִ����out �����Ĵ洢����
        /// </summary>
        /// <param name="connectionString">�����ַ���</param>
        /// <param name="spName">�洢������</param>
        /// <param name="parameterValues">�������</param>
        /// <returns>ִ�н������</returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, spName, parameterValues);
                object val = cmd.ExecuteScalar();

                return val;
            }
        }


        #endregion

        #region Private Method
        /// <summary>
        /// ����һ���ȴ�ִ�е�SqlCommand����
        /// </summary>
        /// <param name="cmd">SqlCommand ���󣬲�����ն���</param>
        /// <param name="conn">SqlConnection ���󣬲�����ն���</param>
        /// <param name="commandText">Sql ���</param>
        /// <param name="cmdParms">SqlParameters  ����,����Ϊ�ն���</param>
        private static void PrepareCommand(SqlCommand cmd, CommandType commandType, SqlConnection conn, string commandText, SqlParameter[] cmdParms)
        {
            //������
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //����SqlCommand����
            cmd.Connection = conn;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// ����һ���ȴ�ִ�д洢���̵�SqlCommand����
        /// </summary>
        /// <param name="cmd">SqlCommand ���󣬲�����ն���</param>
        /// <param name="conn">SqlConnection ���󣬲�����ն���</param>
        /// <param name="spName">Sql ���</param>
        /// <param name="parameterValues">���������Ĵ洢���̲���������Ϊ��</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, string spName, params object[] parameterValues)
        {
            //������
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //����SqlCommand����
            cmd.Connection = conn;
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            //��ȡ�洢���̵Ĳ���
            SqlCommandBuilder.DeriveParameters(cmd);

            //�Ƴ�Return_Value ����
            cmd.Parameters.RemoveAt(0);

            //���ò���ֵ
            if (parameterValues != null)
            {
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    cmd.Parameters[i].Value = parameterValues[i];

                }
            }
        }

        private static object GetDBNullValue(string typeFullName)
        {
            typeFullName = typeFullName.ToLower();
            if (typeFullName == "string")
            {
                return string.Empty;
            }
            else if (typeFullName == "int32" || typeFullName == "int16" || typeFullName == "int64")
            {
                return 0;
            }
            else if (typeFullName == "datetime")
            {
                return Convert.ToDateTime(DateTime.MinValue);
            }
            else if (typeFullName == "boolean")
            {
                return false;
            }
            else if (typeFullName == "int")
            {
                return 0;
            }
            else
            {
                return null;
            }
        }
        #endregion

    }
}