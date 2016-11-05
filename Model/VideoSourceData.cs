using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using AcFunVideo.Class;
using System.IO;

namespace AcFunVideo.Model
{
    public class VideoSourceData : AcFunVideoData
    {
         public  VideoSourceData()
        {
            base.isVideoData = true;
        }


        public async override Task<bool> ParseVideoData(string str)
        {
            try
            {
                JObject obje = JObject.Parse(str);
                var data = obje["data"];
                var vd = new VideoDetail();
                vd.SourceType = data["source"].ToString();
                vd.SourceId = data["sourceId"].ToString();
                vd.Title = data["title"].ToString();
                vd.Time = data["totalseconds"].ToString();
               return await GetRealUrl(vd);
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
                return false;
            }
            return true;
        }

        private async Task<bool> GetRealUrl(VideoDetail vd)
        {
            try
            {
                var url = GetEncodeUrl(vd.SourceId);
                HttpEngine he = new HttpEngine();
                var data = await he.Get(url);
                var reader = new StreamReader(data.AsStreamForRead());
                var str = reader.ReadToEnd();
                var objc = Newtonsoft.Json.Linq.JObject.Parse(str);
                var keystr = objc["data"].ToString();
                var destr = Utils.ACAESDecode(keystr, "qwer3as2jin4fdsa");
                GetPlayUrlNotEncode(destr);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        private string GetEncodeUrl(string vid)
        {
            var time = Utils.currentTimeMillis();
            var md5str = Utils.GetMD5String("GET:/common/partner/play:" + time + ":78554907b127c3853f8e956243dc74c4");
            if (DetailsPage.did.Length < 2)
            {

                DetailsPage.did = System.Guid.NewGuid().ToString().Replace("-", "");
            }
            var guid = System.Guid.NewGuid().ToString().Replace("-", "");
            var url = "http://acfun.api.mobile.youku.com/common/partner/play?_t_=" + time + "&e=md5&_s_=" + md5str + "&point=1&id=" + vid + "&local_time=&point=1&format=1,5,6,7,8&language=guoyu&did=" + DetailsPage.did + "&ctype=87&local_point=&audiolang=1&pid=528a34396e9040f3&guid=" + guid + "&network=WIFI";
            return url;
        }

        private void GetPlayUrlNotEncode(string str)
        {
            try
            {
                JObject obj = JObject.Parse(str);
                var results = obj["results"];
                var mp4 = results["mp4"];
                var hd2 = results["hd2"];
                var m3u8 = results["m3u8"];
                var flvhd = results["flvhd"];
                var sidData = obj["sid_data"];

                base.ListofVideoSourceData = new Dictionary<string, VideosSource>();
                var mp4vs = mp4.ToACVS();
                GetSid(sidData, mp4vs);
                mp4vs.EncodeUrl();
                var hd2vs = hd2.ToACVS();
                GetSid(sidData, hd2vs);
                hd2vs.EncodeUrl();
                var flvhdvs = flvhd.ToACVS();
                GetSid(sidData, flvhdvs);
                flvhdvs.EncodeUrl();
            
                if (mp4vs != null)
                {
                    base.ListofVideoSourceData.Add("标清", mp4vs);
                }
                if (flvhdvs != null)
                {
                    ListofVideoSourceData.Add("高清", flvhdvs);
                }
                if (hd2vs != null)
                {
                    ListofVideoSourceData.Add("超清", hd2vs);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetSid(JToken sidToken, VideosSource vs)
        {
            if (sidToken == null || vs == null)
            {
                return;
            }
            else
            {
                foreach (var acurl in vs.AcUrl)
                {
                }
                if (vs.AcUrl.Count == null)
                {
                    return;
                }
                for (int i = 0; i < vs.AcUrl.Count; i++)
                {
                    if (vs.AcUrl[i].SidData == null)
                    {
                        vs.AcUrl[i].SidData = new VideoSidData();
                    }
                    vs.AcUrl[i].SidData.Oip = sidToken["oip"].ToString();
                    vs.AcUrl[i].SidData.Token = sidToken["token"].ToString();
                    vs.AcUrl[i].SidData.Sid = sidToken["sid"].ToString();
                }
            }
        }

    }

    public static class EJToken
    {
        public static VideosSource ToACVS(this JToken token)
        {
            try
            {
                if (token.Children().Count() < 1)
                {
                    return null;
                }
                VideosSource vs = new VideosSource();
                var urls = token.Children()["url"].ToList();
                var fileids = token.Children()["fileid"].ToList();
                var times = token.Children()["seconds"].ToList();
                var sizes = token.Children()["size"].ToList();
                vs.AcUrl = new List<ACUrl>();
                foreach (var url in urls)
                {
                    ACUrl acurl = new ACUrl();
                    acurl.url = url.ToString();
                    vs.AcUrl.Add(acurl);
                }
                if (fileids == null)
                {
                    return null;
                }
                for (int i = 0; i < fileids.Count; i++)
                {
                    if (vs.AcUrl[i].SidData == null)
                    {
                        vs.AcUrl[i].SidData = new VideoSidData();
                    }
                    vs.AcUrl[i].SidData.FileId = fileids[i].ToString();

                    try
                    {
                        vs.AcUrl[i].size = int.Parse(sizes[i].ToString());
                        vs.AcUrl[i].Time = int.Parse(times[i].ToString());
                    }
                    catch
                    {
                    }
                }
                return vs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void EncodeUrl(this VideosSource vs)
        {
            if (vs == null)
            {
                return;
            }
            List<string> urls = new List<string>();
            foreach (var url in vs.AcUrl)
            {
                var ep = Utils.ACAESENCODE(url.SidData.Sid + "_" + url.SidData.FileId + "_" + url.SidData.Token, "zx26mfbsuebv72ja");
                ep = Uri.EscapeDataString(ep);
                var str = url.url + "&ep=" + ep + "&hd=2&oip=" + url.SidData.Oip + "&sid=" + url.SidData.Sid + "&token=" + url.SidData.Token + "&ctype=87&ev=1&did=" + DetailsPage.did;
                urls.Add(str);
            }
            vs.RealUrl = urls;

            for (int i = 0; i < urls.Count; i++)
            {
                vs.AcUrl[i].url = urls[i];
            }
        }
    }


}
