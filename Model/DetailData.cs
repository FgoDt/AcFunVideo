using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
   public class DetailData:AcFunVideoData
    {
        public async override void ParseData(string str)
        {
            try
            {
                JObject obj = JObject.Parse(str);
                var data = obj["data"];
                var owner = obj["data"]["owner"];
                JEnumerable<JToken> tags = new JEnumerable<JToken>();
                var videos = obj["data"]["videos"].ToList();
                var visit = obj["data"]["visit"];

                base.ListOfACContent = new List<AcContent>();
                var ac = new AcContent();

                //root data
                ac.ChannelId = data["channelId"].ToString();
                ac.ContentId = data["contentId"].ToString();
                ac.Cover = data["cover"].ToString();
                ac.Description = data["description"].ToString();
                ac.Display = data["display"].ToString();
                ac.IsArticle = data["isArticle"].ToString();
                ac.ReleaseDate = data["releaseDate"].ToString();
                ac.Title = data["title"].ToString();
                ac.Toplevel = data["topLevel"].ToString();
                ac.UpdatedAt = data["updatedAt"].ToString();

                //owner data
                ac.User = new ACUser();
                base.ParseOwner(owner, ac);

                //tags data
                ac.Tags = new List<string>();
                foreach (var tag in tags)
                {
                    ac.Tags.Add(tag.ToString());
                }
                //video data
                ac.DetailVideos = new List<VideoDetail>();
                foreach (var video in videos)
                {
                    var v = new VideoDetail();
                    v.CommentId = video["commentId"].ToString();
                    v.DanmakuId = video["danmakuId"].ToString();
                    v.SourceId = video["sourceId"].ToString();
                    v.SourceType = video["sourceType"].ToString();
                    v.StartTime = video["startTime"].ToString();
                    v.Time = video["time"].ToString();
                    v.Title = video["title"].ToString();
                    v.Url = video["url"].ToString();
                    v.VideoId = video["videoId"].ToString();
                   // v.VisibleLevel = video["visibleLevel"].ToString();
                    ac.DetailVideos.Add(v);
                }

                //visit data
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
