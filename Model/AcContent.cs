using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;

namespace AcFunVideo.Model
{
    public class ACUser
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string UserImg { get; set; }
    }

    public class ACVisit
    {
        public string Comments { get; set; }
        public string DanmakuSize { get; set; }
        public string GoldBanana { get; set; }
        public string Score { get; set; }
        public string Stows { get; set; }
        public string Ups { get; set; }
        public string Views { get; set; }
    }

    public class Bangumi
    {
        public string BangumiId { get; set; }
        public string Sort { get; set; }
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string Week { get; set; }
        public string LastVideoName { get; set; }
        public string Month { get; set; }
        public string CopyRight { get; set; }
        public string PlayWay { get; set; }
        public string Year { get; set; }
    }

    public class VideoDetail
    {
        public string AllowDanmaku { get; set; }
        public string CommentId { get; set; }
        public string DanmakuId { get; set; }
        public string SourceId { get; set; }
        public string SourceType { get; set; }
        public string StartTime { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string ACTitle { get; set; }
        public string Url { get; set; }
        public string VideoId { get; set; }
        public string VisibleLevel { get; set; } 
        public string BangumiId { get; set; }
        public string sort { get; set; }
        public string UpdateTime { get; set; }
    }

    public class VideoSidData
    {
        public string Token { get; set; }
        public string Oip { get; set; }
        public string Sid { get; set; }
        public string FileId { get; set; }
    }

    public class ACUrl
    {
        public string url { get; set; }
        public int size { get; set; }
        public int Time {get;set;}
        public VideoSidData SidData { get; set; }
    }

    public class VideosSource
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ACUrl> AcUrl { get; set; }
        public List<string> RealUrl { get; set; }
        //public Dictionary<string,string> Url { get; set; }
    }

    

    public class AcContent
    {
        /// <summary>
        /// 色 无码
        /// </summary>
        public AcContentType AcContentType { get; set; }
        public string Avatar { get; set; }
       
        public string Type { get; set; }
        public string CoverHorizonTal { get; set; }
        public string ChannelId { get; set; }

        private string _intro;
        public string Intro
        {
            get
            {
                return _intro;
            }
            set
            {
                _intro = value;
                Description = value;
            }
        }
        public string Display { get; set; }
        public string Description { get; set; }
        private string _contentId;

        public string ContentId
        {
            get
            {
                return _contentId;
            }
            set
            {
                int result;
                if (value != null&&int.TryParse(Regex.Match(value,@"\d+").Value,out result))
                {
                    _contentId = result.ToString();
                }
            }
        }

        public string ReleaseDate { get; set; }
        public string IsArticle { get; set; }
        public string IsRecommend { get; set; }
        public string Title { get; set; }
        private string _titleImg;

        public string TitleImg
        {
            get { return _titleImg; }
            set
            {
                _titleImg = value;
                Cover = value;
            }
        }
        public string Status { get; set; }
        public string UpdatedAt { get; set; }
        public string Cover { get; set; }
        public string ViewOnly { get; set; }
        public string Toplevel { get; set; }
        public string TudouDomain { get; set; }
        public string ACURL{get;set ;}
        //[JsonIgnore] 
        public List<string> Tags { get; set; }
        private string _id;

        public string id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _contentId = value;
            }
        }
        public string UserName { get; set; }
        public ACUser User { get; set; }
        public ACVisit Visit { get; set; }
        public Bangumi Bangumi { get; set; }
        public List<VideoDetail> DetailVideos { get; set; }

        public int Stows { get; set; }
        public int Views { get; set; }
        public int Comments { get; set; }

    }

    public class ChannelContent : AcContent
    {
        public List<ChannelContent> ChildChannels { get; set; }
        public List<AcContent> ACContetns { get; set; }
        public string ConfigRegion { get; set; }
        public string pid { get; set; }
        public string Updater { get; set; }
    }

    public enum DanmuLocation
    {
        normal,
        top,
        bottom
    }
    public class DanmuContent
    {
        public string Color { get; set; }
        public string Size { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public string Msg { get; set; }

        public SolidColorBrush RealColor { get { return getColor(); } }
        public int RealSize { get { return getSize(); } }
        public DanmuLocation RealLocation { get { return getLocation(); } }
        public double RealTime { get { return getTime(); } }

        private SolidColorBrush getColor()
        {

            SolidColorBrush result = new SolidColorBrush( Windows.UI. Color.FromArgb(255,255,255,255));
            try
            {
                byte[] colorBytes = new byte[3];
                var inss = Convert.ToInt32(this.Color);
                colorBytes[0] = (byte)((inss >> 16) & 0xff);
                colorBytes[1] = (byte)((inss >> 8) & 0xff);
                colorBytes[2] = (byte)(inss & 0xff);
                result = null;
                result = new SolidColorBrush(Windows.UI.Color.FromArgb(255, colorBytes[0], colorBytes[1], colorBytes[2]));
                return result;
            }
            catch 
            {
            }
            return result;
        }

        private int getSize()
        {
            try
            {
                return Int32.Parse(this.Size);
            }
            catch 
            {
            }
            return 25;
        }
        private DanmuLocation getLocation()
        {
            try
            {
                if (this.Location=="1")
                {
                    return DanmuLocation.normal;
                }
                else if(this.Location=="4")
                {
                    return DanmuLocation.bottom;
                }
                else
                {
                    return DanmuLocation.top;
                }
            }
            catch 
            {
            }
            return DanmuLocation.normal;
        }

        private double getTime()
        {
            try
            {
              return 1000* Double.Parse(this.Time);
            }
            catch 
            {
            }
            return 0;
        }
    }

    public enum AcContentType
    {
        /// <summary>
        /// 视频
        /// </summary>
        Videos,

        /// <summary>
        /// 番剧
        /// </summary>
        Bangumis,

        /// <summary>
        /// 文章
        /// </summary>
        Articles,

        /// <summary>
        /// 合集
        /// </summary>
        Special,
        /// <summary>
        /// UP主
        /// </summary>
        User,
    }

    public class ACCollection : System.Collections.ObjectModel.ObservableCollection<object>
    {
        public ACCollection()
            : base()
        {

        }
    }


}
