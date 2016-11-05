using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcFunVideo.Class;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    public class HomeData
    {
        private Dictionary<string, AXFContent> DataOfDic = new Dictionary<string, AXFContent>();
        async public Task<Dictionary<string,AXFContent>> GetData()
        {

           await this.GetHttpData();
            return DataOfDic;
        }

        private async Task<int> GetHttpData()
        {
            HttpEngine he = new HttpEngine();
            var stream = await he.Get(AcFunAPI.GetHomeDataUrl());
            if (stream != null)
            {
                var str = new System.IO.StreamReader(stream.AsStreamForRead()).ReadToEnd();
                ParseData(str);
            }
            return 0;
        }


        private void GetVideoData(JToken item)
        {
            AXFContent axfc = new AXFContent();
            axfc.Belong = item["belong"].ToString();
            axfc.ChannelId = item["channelId"].ToString();
            axfc.ContentCount = item["contentCount"].ToString();
            axfc.Id = item["id"].ToString();
            axfc.IMG = item["image"].ToString();
            axfc.Name = item["name"].ToString();
            var contents = item["contents"].Children();
            foreach (var content in contents)
            {
                var ac = new AcContent();
                try
                {
                    ac.id = content["id"].ToString();
                }
                catch 
                {
                }
                ac.Cover = content["image"].ToString();
                ac.Title = content["title"].ToString();
                ac.ContentId = content["url"].ToString();
                if (axfc.Contents == null)
                {
                    axfc.Contents = new List<AcContent>();
                }
                axfc.Contents.Add(ac);
            }
            DataOfDic.Add(axfc.Name, axfc);
        }

        public async Task<AXFContent> GetSingleData(string id , string key)
        {
            HttpEngine he = new HttpEngine();
            var stream = await he.Get(AcFunAPI.GetRegionUrlById(id));
            if (stream != null)
            {
                var str = new System.IO.StreamReader(stream.AsStreamForRead()).ReadToEnd();
                ParseSingleData(str);

            }
            return DataOfDic[key];
        }

        private void ParseSingleData(string str)
        {
            if (str != null)
            {
                JObject obj = JObject.Parse(str);
                var datas = obj["data"];
                    GetVideoData(datas);
            }
        }

        private void GetBannerData(JToken item)
        {
            AXFContent axfc = new AXFContent();
            axfc.Belong = item["belong"].ToString();
            axfc.ChannelId = item["channelId"].ToString();
            axfc.ContentCount = item["contentCount"].ToString();
            axfc.Id = item["id"].ToString();
            axfc.IMG = item["image"].ToString();
            axfc.Name = item["name"].ToString();
            DataOfDic.Add(axfc.Name, axfc);
        }

        private void ParseData(string str)
        {
            if (str != null)
            {

                JObject obj = JObject.Parse(str);
                var datas = obj["data"].Children();
                foreach (var item in datas)
                {
                    switch (item["name"].ToString())
                    {
                        case "轮播图":
                            GetVideoData(item);
                            break;
                        case "热门推荐":
                            GetVideoData(item);
                            break;

                        default:
                            GetBannerData(item);
                            break;
                    }
                }
            }
        }
    }
}
