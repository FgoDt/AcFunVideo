using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcFunVideo.Class;

namespace AcFunVideo.Model
{
    class ACArticleData : AcContent
    {
        public string Txt { get; set; }
        private AcContent acContent;
        public ACArticleData(AcContent content)
        {
            this.acContent = content;
        }
        public ACArticleData GetData()
        {
            return this;
        }

        private void GetHttpData()
        {
            if (this.acContent!=null)
            {
                var url=AcFunAPI.GetArticleContentUrl(this.acContent.ContentId);
            }
        }
    }
}
