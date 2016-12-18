using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Helper
{
    /// <summary>  
    /// Http连接操作帮助类  
    /// </summary>
    public static class HttpHelper
    {
        #region 预定义方变量  
        //默认的编码  
        private static Encoding encoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求  
        private static HttpWebRequest request = null;
        //获取影响流的数据对象  
        private static HttpWebResponse response = null;
        #endregion

        public static string GetHtml(HttpItem item)
        {
            try
            {
                //设置参数
                SetRequest(item);
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    string htmlResponse = string.Empty;
                    Stream stream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(stream, item.Encoding))
                    {
                        htmlResponse = reader.ReadToEnd();
                    }
                    return htmlResponse;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private static void SetRequest(HttpItem item)
        {
            request = (HttpWebRequest)HttpWebRequest.Create(item.URL);
            request.Method = item.Method;
            request.Timeout = item.Timeout;
            request.Accept = item.Accept;
            request.ContentType = item.ContentType;
            request.UserAgent = item.UserAgent;
        }
        /// <summary>  
        /// Http请求参考类  
        /// </summary>  
        public class HttpItem
        {
            string _URL = string.Empty;
            /// <summary>  
            /// 请求URL必须填写  
            /// </summary>  
            public string URL
            {
                get { return _URL; }
                set { _URL = value; }
            }
            string _Method = "GET";
            /// <summary>  
            /// 请求方式默认为GET方式,当为POST方式时必须设置Postdata的值  
            /// </summary>  
            public string Method
            {
                get { return _Method; }
                set { _Method = value; }
            }
            int _Timeout = 100000;
            /// <summary>  
            /// 默认请求超时时间  
            /// </summary>  
            public int Timeout
            {
                get { return _Timeout; }
                set { _Timeout = value; }
            }
            string _Accept = "text/html, application/xhtml+xml, */*";
            /// <summary>  
            /// 请求标头值 默认为text/html, application/xhtml+xml, */*  
            /// </summary>  
            public string Accept
            {
                get { return _Accept; }
                set { _Accept = value; }
            }
            string _ContentType = "text/html";
            /// <summary>  
            /// 请求返回类型默认 text/html  
            /// </summary>  
            public string ContentType
            {
                get { return _ContentType; }
                set { _ContentType = value; }
            }
            string _UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            /// <summary>  
            /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)  
            /// </summary>  
            public string UserAgent
            {
                get { return _UserAgent; }
                set { _UserAgent = value; }
            }
            Encoding _Encoding = Encoding.Default;
            /// <summary>  
            /// 返回数据编码默认为NUll,可以自动识别,一般为utf-8,gbk,gb2312  
            /// </summary>  
            public Encoding Encoding
            {
                get { return _Encoding; }
                set { _Encoding = value; }
            }
            private bool isToLower = false;
            /// <summary>  
            /// 是否设置为全文小写，默认为不转化  
            /// </summary>  
            public bool IsToLower
            {
                get { return isToLower; }
                set { isToLower = value; }
            }
        }
    }
}
