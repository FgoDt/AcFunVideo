using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace AcFunVideo.View
{
   public class ACDownloadOperation
    {
        public DownloadOperation Operation { get; set; }
        public AcFunVideo.Model.AcContent Item { get; set; }
    }
}
