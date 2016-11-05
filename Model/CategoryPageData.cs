using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    public class CategoryPageData:ACChannelsData
    {

    

        public override void ParseData(string str)
        {

            try
            {
                var obj = JObject.Parse(str);
                var data = obj["data"].ToList();
                foreach (var da in data)
                {
                  var ch=  this.ParseEachData(da);
                    this.dicOfData.Add(ch.Title, ch);
                }
            }
            catch ( Exception ex)
            {
            }
        }

       private ChannelContent ParseEachData(JToken token)
        {
            ChannelContent accd = new ChannelContent();
            try
            {
                var belong = token["belong"].ToString();
                var channelid = token["channelId"].ToString();
                var Contents = token["contents"];
                var id = token["id"].ToString();
                var image = token["image"].ToString();
                var name = token["name"].ToString();
                var type = token["type"]["value"].ToString();
                accd.id = id;
                accd.Cover = image;
                accd.Type = type;
                accd.Title = name;
                if (Contents!=null)
                {
                    foreach (var  content in Contents)
                    {
                        if (accd.ACContetns==null)
                        {
                            accd.ACContetns = new List<AcContent>();
                        }

                      var ac=  this.ParseContentsData(content,type);
                        ac.Type = type;
                        accd.ACContetns.Add(ac);
                    }
                }

            }
            catch 
            {
            }
            return accd;
        }

        private  AcContent ParseContentsData(JToken token,string type)
        {
            AcContent ac = new AcContent();
            try
            {
              //  var id = token["id"].ToString();
                var image = token["image"].ToString();
                var title = token["title"].ToString();
                var url = token["url"].ToString();
               // ac.id = id;
                ac.Cover = image;
                ac.Title = title;
               ac.ContentId= ac.ACURL = url;
                if (type=="videos")
                {
                    var intro = token["intro"].ToString();
                    ac.Description = intro;
                    base.ParseVisit(token["visit"], ac);
                }
                if (type=="users")
                {
                    //todo
                }

            }
            catch 
            {
            }
            return ac;
        }


    }
}
