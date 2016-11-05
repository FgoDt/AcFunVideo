using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace AcFunVideo.Model
{
    public class ACChannelsData:AcFunVideoData
    {
       public  Dictionary<string, ChannelContent> dicOfData = new Dictionary<string, ChannelContent>();

        public async Task<List<AcContent>> GetLocalData()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/ChannelsData.json", UriKind.Absolute));
                string jsonStr = await FileIO.ReadTextAsync(file);
                this.ParseData(jsonStr);
            }
            catch (Exception ex)
            {
            }

            return base.ListOfACContent;
        }
        public override void ParseData(string str)
        {
            try
            {
                JObject obj = JObject.Parse(str);
                var data = obj["data"].ToList();
                foreach (var channel in data)
                {
                  var cc=  ParseChannel(channel);
                    this.dicOfData.Add(cc.Title, cc);
                } 
            }
            catch (Exception ex)
            {

            }
        }


        private ChannelContent ParseChannel(JToken token)
        {

            ChannelContent cc;
            try
            {
                var channels = token["childChannels"].ToList();
                var id = token["id"].ToString();
                var img = token["img"].ToString();
                var name = token["name"].ToString();
                var pid = token["pid"].ToString();
                var channelId = token["channelid"];
                var contents = token["contents"];
                cc = new ChannelContent();
                cc.id = id;
                cc.Cover = img;
                cc.Title = name;
                cc.pid = pid;
                if (contents!=null)
                {
                }
                foreach (var channel in channels)
                {
                    if (cc.ChildChannels==null)
                    {
                        cc.ChildChannels = new List<ChannelContent>();
                    }
                   var ccc= this.ParseChannel(channel);
                    cc.ChildChannels.Add(ccc);
                }
            }
            catch (Exception ex)
            {
                cc = null;
            }
            return cc;
        }
    }

}
