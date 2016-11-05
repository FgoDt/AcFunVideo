using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    class BangumiData:AcFunVideoData
    {
        public override void ParseData(string str)
        {
            try
            {
                JObject obj = JObject.Parse(str);
                ListOfACContent = new List<AcContent>();
                var msg = obj["message"];
                if (msg.ToString().ToUpper() == "OK")
                {
                    var contents = obj["data"]["contents"].ToList();
                    foreach (var item in contents)
                    {
                        var ac = new AcContent();
                        ac.ChannelId = item["channelId"].ToString();
                        ac.id = item["id"].ToString();
                        ac.Cover = item["image"].ToString();
                        ac.Description = item["intro"].ToString();
                        ac.ReleaseDate = item["releasedAt"].ToString();
                        ac.Title = item["title"].ToString();
                        ac.ACURL = item["url"].ToString();

                        var bangumi = item["latestBangumiVideo"];
                        var visit = item["visit"];
                        if (bangumi!=null)
                        {
                            ac.Bangumi = new Bangumi();
                            this.ParseBangumiData(bangumi, ac);
                        }
                        if (visit!=null)
                        {
                            ac.Visit = new ACVisit();
                            base.ParseVisit(visit, ac);
                        }
                        base.ListOfACContent.Add(ac);
                    }
                }
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }

        private void ParseBangumiData(JToken bangumi,AcContent ac)
        {
            ac.Bangumi.BangumiId = bangumi["bangumiId"].ToString();
            ac.Bangumi.Sort = bangumi["sort"].ToString();
            ac.Bangumi.Title = bangumi["title"].ToString();
            ac.Bangumi.VideoId = bangumi["videoId"].ToString();
            base.TestProtectedFunc();
        }

       
      
    }
}
