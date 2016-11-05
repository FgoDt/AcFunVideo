using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcFunVideo.Model
{
   public class commentContent
    {
        public string cid { get; set; }
        public string quoteId { get; set; }
        public string content { get; set; }
        public string userID { get; set; }
        public string userName { get; set; }
        public Uri userImg { get; set; }
        public float deep { get; set; }
        public float refCount { get; set; }
        public string nameRed { get; set; }
        public int isRef { get; set; }
        public string count { get; set; }
        public string postDate { get; set; }
    }

    public class acdata
    {
        public List<string> commentList { get; set; }
        public float totalPage { get; set; }
        public float pageSize { get; set; }
        public float totalCount { get; set; }
        public Dictionary<string, commentContent> commentContentArr { get; set; }
    }
    public class ACComment
    {
        public bool sucess { get; set; }
        public string msg { get; set; }
        public int status { get; set; }
        public acdata data { get; set; }
    }
}
