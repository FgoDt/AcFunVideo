using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    public class BangumiDetailData : AcFunVideoData
    {
        public override void ParseData(string str)
        {

            try
            {
                var obj = JObject.Parse(str);
                var data = obj["data"];
                var latestVideo = obj["data"]["latestVideo"];
                var tags = obj["data"]["tags"].ToList();
                var videos = obj["data"]["videos"].ToList();
                var visit = obj["data"]["visit"];

                base.ListOfACContent = new List<AcContent>();
                var ac = new AcContent();
                ac.Bangumi = new Bangumi();

                //base data
                ac.Bangumi.BangumiId = data["bangumiId"].ToString();
                ac.Cover = data["cover"].ToString();
                ac.Bangumi.CopyRight = data["copyright"].ToString();
                ac.Description = data["intro"].ToString();
                ac.Bangumi.Month = data["month"].ToString();
                ac.Title = data["title"].ToString();
                ac.Bangumi.Week = data["week"].ToString();
                ac.Bangumi.PlayWay = data["playWay"].ToString();
                ac.Bangumi.Year = data["year"].ToString();

                //latest video
                //todo

                //tags
                //todo

                //videos
                ac.DetailVideos = new List<VideoDetail>();
                foreach (var video in videos)
                {
                    VideoDetail vd = new VideoDetail();
                    vd.BangumiId = video["bangumiId"].ToString();
                    vd.DanmakuId = video["danmakuId"].ToString();
                    vd.SourceId = video["sourceId"].ToString();
                    vd.SourceType = video["sourceType"].ToString();
                    vd.Title = video["title"].ToString();
                    vd.VideoId = video["videoId"].ToString();
                    vd.VisibleLevel = video["visibleLevel"].ToString();
                    //vd.UpdateTime = video["updateTime"].ToString();
                    ac.DetailVideos.Add(vd);
                }


                //visit 
                ac.Visit = new ACVisit();
                base.ParseVisit(visit, ac);
                base.ListOfACContent.Add(ac);
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }
    }
}
