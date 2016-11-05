using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
   public class DanmuData:AcFunVideoData
    {
       public  List<DanmuContent> ListOfDanmu = new List<DanmuContent>();
        public string DanmaKuId { get; set; }
        private int PaeNo = 1;

        private async void GetNextData()
        {
           await  base.GetData(AcFunVideo.Class.AcFunAPI.GetDanmuUrl(DanmaKuId, PaeNo));
        }

        public override void ParseData(string str)
        {

            try
            {
                JArray obj = JArray.Parse(str);

                foreach (var array in obj)
                {
                    var arrays = array.ToList();
                    foreach (var item in arrays)
                    {
                        var c = item["c"].ToString().Split(',');
                        var m = item["m"].ToString();
                        DanmuContent dc = new DanmuContent();
                        dc.Time = c[0];
                        dc.Color = c[1];
                        dc.Location = c[2];
                        dc.Size = c[3];
                        dc.Msg = m;
                        ListOfDanmu.Add(dc);
                    }
                }
                if (ListOfDanmu.Count>500*PaeNo)
                {
                    PaeNo++;
                    GetNextData();
                }
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }

            //base.ParseData(str);
        }
    }
}
