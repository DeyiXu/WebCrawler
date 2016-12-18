using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebCrawler.DAL
{
    public class DALBase
    {
        protected internal const string INSERTID = "@InsertId";
        protected internal const string SELECT_COLLATE = " COLLATE Chinese_PRC_CS_AI;";
    }
}
