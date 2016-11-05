using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace AcFunVideo.Model
{
  public  class NewBangumiData:AcFunVideoData
    {
        public bool isTV { get; set; }
        public override void ParseData(string str)
        {
            JObject obj = JObject.Parse(str);
            try
            {
                base.ListOfACContent = new List<AcContent>();
                var datas = obj["data"]["list"].ToList();
                foreach (var item in datas)
                {
                    var ac = new AcContent();
                    ac.Bangumi = new Bangumi();
                    ac.Cover = item["cover"].ToString();
                    ac.Bangumi.BangumiId = item["id"].ToString();
                    ac.Bangumi.Week = item["week"].ToString();
                    ac.Title = item["title"].ToString();
                    //ac.Description = item["intro"].ToString();
                    ac.Bangumi.Title= ac.Bangumi.LastVideoName = item["lastVideoName"].ToString();
                    ac.UpdatedAt = item["lastUpdateTime"].ToString();

                    if (isTV)
                    {
                        var acid = item["linkUrl"].ToString();
                      ac.id=  ParseTVData(acid);
                    }

                    base.ListOfACContent.Add(ac);
                }
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }

        private string ParseTVData(string linkUrl)
        {
          var selec=  new Regex(@"\bac\d{2,10}");
            var match = selec.Match(linkUrl);
            var id = match.ToString().Substring(2, match.Length - 2);
            return id;
        }
    }
}
