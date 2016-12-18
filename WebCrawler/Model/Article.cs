using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Model
{
    public class Article
    {
        public int Id { get; set; }
        private string _articleUrl = string.Empty;
        private string _title = string.Empty;
        private string _keywords = string.Empty;
        private string _description = string.Empty;
        private string _content = string.Empty;
        private string _tags = string.Empty;
        private DateTime _createTime = DateTime.MinValue;
        public string ArticleUrl
        {
            get
            {
                return _articleUrl;
            }

            set
            {
                _articleUrl = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }

        public string Keywords
        {
            get
            {
                return _keywords;
            }

            set
            {
                _keywords = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
            }
        }

        public string Tags
        {
            get
            {
                return _tags;
            }

            set
            {
                _tags = value;
            }
        }

        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }

            set
            {
                _createTime = value;
            }
        }


    }
}
