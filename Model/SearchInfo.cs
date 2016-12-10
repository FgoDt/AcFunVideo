using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AcFunVideo.Model
{
    
    class RespData<T>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }
    }

    class RespPage<T>
    {
        public T Page { get; set; }
    }

    class RespList<T>
    {
        public int TotalCount { get; set; }
        public T List { get; set; }
        public T User { get; set; }
    }

    public class Hotword
    {
        public int Orderby { get; set; }
        public string Value { get; set; }
    }

    public class Suggest
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }
    public class SearchInfo
    {
        private List<AcContent> _bangumis;
        [JsonProperty("ai")]
        public List<AcContent> Bangumis
        {
            get
            {
                return _bangumis;
            }
            set
            {
                if (value != null)
                {
                    _bangumis = value;
                    foreach (AcContent t in _bangumis)
                    {
                        t.AcContentType = AcContentType.Bangumis;
                        if (t.Bangumi == null)
                        {
                            t.Bangumi=new Bangumi
                            {
                                BangumiId = t.ContentId,
                                Title = t.Title
                            };
                        }
                    }
                }
            }
        }
        [JsonProperty("list")]
        public List<AcContent> Videos { get; set; }
        [JsonProperty("green")]
        public List<AcContent> Articles { get; set; }
        [JsonProperty("sp")]
        public List<AcContent> Special { get; set; }
        [JsonProperty("user")]
        public List<AcContent> Users { get; set; }
    }

     

    public enum SortType
    {
        /// <summary>
        /// 相关度
        /// </summary>
        Score,
        /// <summary>
        /// 最多人观看
        /// </summary>
        Views,
        /// <summary>
        /// 评论最多
        /// </summary>
        Comments,
        /// <summary>
        /// 收藏最多
        /// </summary>
        Stows,
        /// <summary>
        /// 最新发布
        /// </summary>
        ReleaseDate
    }

    public class SearchUrlData
    {
        public AcContentType ContentType { get; set; }
        public string Url { get; set; }
    } 
}
