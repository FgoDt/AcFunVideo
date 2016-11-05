using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcFunVideo.Class;
using Windows.Storage;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
    public class MainPageCategoryData
    {
        public string img { get; set; }
        public string title { get; set; }
        public Windows.UI.Xaml.Media.SolidColorBrush borderColor { get; set; }
        public Windows.UI.Xaml.Media.SolidColorBrush backColor { get; set; }

    }
    public class MainPageCategoryDatas
    {

        public List<MainPageCategoryData> list = new List<MainPageCategoryData>();
        public async Task<bool> GetData()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Data/MainPagePartData.json",UriKind.Absolute));
                string jsonStr = await FileIO.ReadTextAsync(file);
                var obj = JObject.Parse(jsonStr);
                var datas = obj["part"].ToList();
                foreach (var item in datas)
                {
                    var val = new MainPageCategoryData();
                    val.img = item["img"].ToString();
                    val.title = item["title"].ToString();
                    val.borderColor =ParseColor( item["borderColor"].ToString());
                    val.backColor =ParseColor( item["backColor"].ToString());
                    list.Add(val);
                }
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }
        private Windows.UI.Xaml.Media.SolidColorBrush ParseColor(string str)
        {
            switch (str)
            {
                case "red":
                    return ACCOLOR.ACRED;
                case "blu":
                    return ACCOLOR.ACBLU;
                case "deepBlu":
                    return ACCOLOR.ACDEEPBLU;
                case "orange":
                    return ACCOLOR.ACORANGE;
                case "green":
                    return ACCOLOR.ACGREEN;
                case "yellow":
                    return ACCOLOR.ACYELLOW;
                default:
                    return null;
            }
        }
    }
}
