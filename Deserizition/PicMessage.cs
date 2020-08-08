using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deserizition
{
    public class PicMessage
    {
        public string Content { get; set; }
        public Grouppic[] GroupPic { get; set; }
        public string Tips { get; set; }
        public long[] UserID { get; set; }

        public class Grouppic
        {
            public long FileId { get; set; }
            public string FileMd5 { get; set; }
            public int FileSize { get; set; }
            public string ForwordBuf { get; set; }
            public int ForwordField { get; set; }
            public string Url { get; set; }
        }
    }
}
