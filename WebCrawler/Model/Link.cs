using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Model
{
    public class Link
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int Depth { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public StatusAttribute Status { get; set; }
        public bool IsDownload { get; set; }
        public enum StatusAttribute
        {
            /// <summary>
            /// 不可使用
            /// </summary>
            NotUse,
            /// <summary>
            /// 可以使用
            /// </summary>
            IsUse
        }
    }
}
