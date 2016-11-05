using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcFunVideo.Class;
using AcFunVideo.Model;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    public abstract class AcFunVideoData
    {
        public bool isVideoData = false;
        public List<AcContent> ListOfACContent { get; set; }
        public Dictionary<string, VideosSource> ListofVideoSourceData { get; set; }
        public virtual async Task<List<AcContent>> GetData(string url)
        {
            this.ClearData();
            await this.GetHttpData(url);
            return ListOfACContent;
        }


        private void ClearData()
        {
            ListOfACContent = null;
            ListofVideoSourceData = null;
        }

        private async Task<int> GetHttpData(string url)
        {
            HttpEngine he = new HttpEngine();
            var stream = await he.Get(url);
            if (stream != null)
            {
                var str = new System.IO.StreamReader(stream.AsStreamForRead()).ReadToEnd();
                if (isVideoData)
                {
                    await ParseVideoData(str);
                }
                else
                    ParseData(str);
            }
            return 0;
        }

        virtual public void ParseVisit(JToken visit, AcContent ac)
        {
            if (ac.Visit == null)
            {
                ac.Visit = new ACVisit();
            }
            ac.Visit.Comments = visit["comments"].ToString();
            ac.Visit.GoldBanana = visit["goldBanana"].ToString();
            ac.Visit.Score = visit["score"].ToString();
            ac.Visit.Ups = visit["ups"].ToString();
            ac.Visit.Stows = visit["stows"].ToString();
            ac.Visit.Views = visit["views"].ToString();
            try
            {
                ac.Visit.DanmakuSize = visit["danmakuSize"].ToString();
            }
            catch
            {
            }
        }
        protected void TestProtectedFunc()
        {
        }
        virtual public void ParseOwner(JToken owner, AcContent ac)
        {
            if (ac.User == null)
            {
                ac.User = new ACUser();
            }
            ac.User.UserImg = owner["avatar"].ToString();
            ac.User.UserId = owner["id"].ToString();
            ac.User.Username = owner["name"].ToString();
        }

        virtual async public Task<bool> ParseVideoData(string str)
        {
            return true;
        }
        virtual public void ParseData(string str)
        {

            try
            {
                JObject obj = JObject.Parse(str);
                ListOfACContent = new List<AcContent>();
                var msg = obj["message"];
                if (msg.ToString().ToUpper() == "OK")
                {
                    var list = obj["data"]["list"].ToList();
                    foreach (var item in list)
                    {
                        var ac = new AcContent();

                        JToken owner = item["owner"];
                        JToken visit = item["visit"];
                        ac.ChannelId = item["channelId"].ToString();
                        ac.ContentId = item["contentId"].ToString();
                        ac.Cover = item["cover"].ToString();
                        ac.Description = item["description"].ToString();
                        ac.IsArticle = item["isArticle"].ToString();
                        ac.IsRecommend = item["isRecommend"].ToString();
                        ac.ReleaseDate = item["releaseDate"].ToString();
                        ac.Status = item["status"].ToString();
                        ac.Title = item["title"].ToString();
                        ac.UpdatedAt = item["updatedAt"].ToString();
                        if (owner != null)
                        {
                            ac.User = new ACUser();
                            ParseOwner(owner, ac);
                        }
                        if (visit != null)
                        {
                            ac.Visit = new ACVisit();
                            ParseVisit(visit, ac);
                        }
                        ListOfACContent.Add(ac);
                    }
                }
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }
    }

}
