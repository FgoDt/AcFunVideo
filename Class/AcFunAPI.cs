using System;

namespace AcFunVideo.Class
{

    public enum RankSort
    {
        Day,
        Week
    };

    public class AcFunAPI
    {
        readonly static String ACHost = "http://www.acfun.tv/";
        readonly static string ACAPIHost = "http://api.acfun.tv/apiserver/";
        readonly static string AIXIFANAPIHost = "http://api.aixifan.com/";
        readonly static string DanmuHost = "http://danmu.aixifan.com/";
        readonly static string WebViewRequestParam = "iswebview=201714&devicetype=ios&";
        readonly static string ICAOHost = "http://icao.acfun.tv/";

        public static string GetArticleContentUrl(string contentid)
        {
            string result = string.Empty;
            result = AcFunAPI.ACAPIHost + "content/info?contentid=" + contentid;
            return result;
        }

        public static string GetHomeDataUrl()
        {
            string result = AcFunAPI.AIXIFANAPIHost + "regions";
            return result;
        }

        public static string GetAllChannelsIdUrl()
        {
            string result = AcFunAPI.AIXIFANAPIHost + "/channels/allChannels";
            return result;
        }

        public static string GetRegionUrlById(string id)
        {
            string result = AcFunAPI.AIXIFANAPIHost + "regions/" + id;
            return result;
        }

        public static string GetCategoryUrl(string belong, string id)
        {
            string result = AcFunAPI.AIXIFANAPIHost + "regions?belong=" + belong + "&channelId=" + id;
            return result;
        }

        public static string GetCartoonRecommendedUrl()
        {
            string result = AcFunAPI.AIXIFANAPIHost + "regions?belong=12&channelid=156";
            return result;
        }
        public static string GetBananaRecommendUrl()
        {
            string result = AcFunAPI.AIXIFANAPIHost + "ranks/1?app_version=4.1.4&market=appstore&origin=ios&page%5Bnum%5D=1&pagesize=10&range=86400000&resolution=750x1334&sys_name=ios&sys_version=9.2.1";
            return result;
        }

        public static string GetContentUrl(string id)
        {
            string result = string.Empty;
            result = AcFunAPI.AIXIFANAPIHost + "videos/" + id;
            return result;
        }

        public static string GetCommentUrl(string id)
        {
            string result = string.Empty;
            //todo
            return result;
        }

        public static string GetVideoSrcUrl(string videoId)
        {
            string result = string.Empty;
            result = AcFunAPI.AIXIFANAPIHost + "plays/" + videoId;
            return result;
        }

        public static string GetDanmuUrl(string videoId)
        {
            string result = string.Empty;
            result = AcFunAPI.DanmuHost + "V3/" + videoId + "/1/500";
            return result;
        }

        public static string GetDanmuUrl(string videoId, int pageNo, int pageSize = 500)
        {
            string result = string.Empty;
            result = AcFunAPI.DanmuHost + "V3/" + videoId + "/" + pageNo + "/" + pageSize;
            return result;
        }

        public static string GetBangumiContentUrl(string bangumiId)
        {
            string result = string.Empty;
            result = AIXIFANAPIHost + "bangumis/" + bangumiId + "?page={num:1,size:50}";
            return result;
        }

        public static string GetBangumiContentUrl(string bangumiId, int page, int size)
        {
            string result = string.Empty;
            result = AIXIFANAPIHost + "bangumis/" + bangumiId + "?page=" + page + "&size=" + size;
            return result;
        }

        public static string GetHomeBangumiUrl(string week = "0")
        {
            string result = string.Empty;
            if (week != "0")
            {
                result = ACHost + "bangumi/bangumi/week?isWeb=1&isindex=1&pageSize=15&bangumiTypes=1&week=" + week;
                return result;
            }
            result = ACHost + "bangumi/bangumi/week?isWeb=1&isindex=1&pageSize=15&bangumiTypes=1";
            return result;
        }


        public static string GetHomenominateUrl(string week = "0")
        {
            string result = string.Empty;
            if (week != "0")
            {
                result = ACHost + "nominate/newnominate_list.aspx?isWeb=1&isindex=1&pageSize=15&bangumiTypes=1&week=" + week;
                return result;
            }
            result = ACHost + "nominate/newnominate_list.aspx?isWeb=1&isindex=1&pageSize=15&bangumiTypes=1";
            return result;
        }


        public static string GetBannaRankUrl(string channelIDS, RankSort sortKey,int pageNo)
        {
            string result;
            string rankSortKey;
            switch (sortKey)
            {
                case RankSort.Day:
                    rankSortKey = "86400000";
                    break;
                case RankSort.Week:
                    rankSortKey = "604800000";
                    break;
                default:
                    rankSortKey = "86400000";
                    break;
            }
                    result = AIXIFANAPIHost + "/ranks/1?app_version=4.1.8&channelIds="+channelIDS+"&market=appstore&origin=ios&page%5Bnum%5D="+pageNo+"&page%5Bsize%5D=10&range="+rankSortKey+"&resolution=750x1334&sys_name=ios&sys_version=9.2.1";

            return result;
        }

        public static string GetAllRankUrl(string channelids,RankSort sortKey,int pageNo)
        {
            string result;
            string rankSortKey;
            if (channelids=="")
            {
                channelids = "106,107,108,133,86,87,88,89,98,83,145,84,85,165,72,96,162,163,141,121,142,99,100,143,136,137,103,138,139,140,134,135,147,148,91,149,150,151,152,94,95,153,154,93,127,128,129,130,92,131,132";
            }
            switch (sortKey)
            {
                case RankSort.Day:
                    rankSortKey = "86400000";
                    break;
                case RankSort.Week:
                    rankSortKey = "604800000";
                    break;
                default:
                    rankSortKey = "86400000";
                    break;
            }
            result = AIXIFANAPIHost + "/searches/channel?app_version=4.1.8&channelIds=" + channelids + "&market=appstore&origin=ios&pageNo=" + pageNo + "&pageSize=20&range=" + rankSortKey + "&resolution=750x1334&sort=1&sys_name=ios&sys_version=9.2.1";

            return result;
        }

        public static string GetBangumiRankUrl(RankSort range,int pageNo)
        {
            string result;

            string sort = "7";
            switch (range)
            {
                case RankSort.Day:
                    sort = "7";
                    break;
                case RankSort.Week:
                    sort = "8";
                    break;
                default:
                    sort = "7";
                    break;
            }

            result = "http://api.aixifan.com/searches/bangumi?app_version=4.1.8&bangumiTypes=1&market=appstore&origin=ios&pageNo="+pageNo+"&pageSize=20&resolution=750x1334&sort="+sort+"&sys_name=ios&sys_version=9.2.1";
            return result;
        }

        public static string GetSinglePartDataUrl(string sort,int page,string channelid,string startDate,string endDate)
        {
            string result = string.Empty;
            result = ACHost + "list/getlist?channelId="+channelid+"&sort="+sort+"&pageSize=20&pageNo="+page+"&startDate="+startDate+"&endDate="+endDate;
            return result;
        }

        public static String GetCommentUrl(string id,int page=1)
        {
            string result = ACHost + "comment_list_json.aspx?contentId="+id+"&currentPage="+page;
            return result;
        }

    }
}
