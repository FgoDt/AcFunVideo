using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AcFunVideo.Model
{
   public class SinglePartDatas:AcFunVideoData
    {

        public override void ParseData(string str)
        {
            try
            {
                var job = JObject.Parse(str);
                var datas = job["data"]["data"].Children();
                foreach (var data in datas)
                {
                    var ac = new AcContent();
                    ac.Description = data["description"].ToString();
                    ac.Title = data["title"].ToString();
                    ac.ContentId = data["id"].ToString();
                    ac.Cover = data["coverImage"].ToString();
                    ac.Visit = new ACVisit();
                    ac.Visit.Views = data["viewCount"].ToString();
                    //if (base.ListOfACContent==null)
                    //{
                    //    base.ListOfACContent = new List<AcContent>();
                    //}
                    //base.ListOfACContent.Add(ac);
                    base.CollectionOfACContent.Add(ac);
                }
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
           // base.ParseData(str);
        }
    }
}
