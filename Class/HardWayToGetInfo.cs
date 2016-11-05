using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcFunVideo.Model;
using Windows.Data.Html;

//handler.proxy=function(e)
//{
//    e = compatible(e);
//    if (e.isImmediatePropagationStopped())
//        return;
//    e.data = data;
//    var result = callback.apply(element, e._args == undefined?[e]:[e].concat(e._args));
//if(result===false)
//    e.preventDefault(),e.stopPropagation();
//    return result
//    }

namespace AcFunVideo.Class
{
   public class HardWayToGetInfo
    {
        string Str;
        public HardWayToGetInfo(string str)
        {
            this.Str = str;
        }
        public void GetInfo(string str)
        {

            var index = str.IndexOf("id=\"block-lightbox\"");
            var t= str.IndexOf("ul");
            var tl= str.IndexOf("ul", index);
            var tll = str.IndexOf("ul", tl + 5);
            var needStr = str.Substring(tl, tll);
        }

        public AcContent GetBannerData()
        {
            AcContent ac = new AcContent();
            if (Str != null && Str != string.Empty)
            {
               var index= Str.IndexOf("header-banner");
                var aindex = Str.IndexOf("<a", index);
                var aend = Str.IndexOf("</a>", index);
                var spanhead = Str.IndexOf("<span", index);
                var spanEnd = Str.IndexOf("</span>", index);

                if (aindex!=-1||aend!=-1||spanhead!=-1||spanEnd!=-1)
                {
                    var astr = Str.Substring(aindex, aend-aindex);
                    var span = Str.Substring(spanhead, spanEnd-spanhead);
                    
                    if (astr == string.Empty || span == string.Empty)
                    {
                        return ac;
                    }
                    var href = astr.IndexOf("href=");
                    var hrefEnd = astr.IndexOf("\"", href + 10);
                    var url = astr.Substring(href + 6, hrefEnd - href - 6);
                    ac.ACURL = url;

                    var spanstr = HtmlUtilities.ConvertToText(span);
                    ac.Title = spanstr;
                    return ac;
                }
 
            }
            return ac;

        }
    }
}
