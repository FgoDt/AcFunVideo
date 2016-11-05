using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcFunVideo.Model
{

    public class AXFType
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

    }

    public class AXFContent
    {
        public string Belong { get; set; }
        public string ChannelId { get; set; }
        public string ContentCount { get; set; }
        public string GoText { get; set; }
        public string Hide { get; set; }
        public string Id { get; set; }
        public string MenuCount { get; set; }
        public string Name { get; set; }
        public string PlatFormId { get; set; }
        public string ShowLine { get; set; }    
        public string ShowMore { get; set; }
        public string ShowName { get; set; }    
        public string Url { get; set; }
        public string IMG { get; set; }
        public List<AcContent> Contents { get; set; }
        public AXFType Type { get; set; }
    }
}
