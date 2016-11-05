using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;

namespace AcFunVideo.Model
{
    public class DownloadData
    {
        public string DMFile { get; set; }
        public List<string> VideoFiles { get; set; }
        public List<string> DownloadGuid { get; set; }
        public VideoDetail content { get; set; }
        public string Pre { get; set; }
        public List<DownloadOperation> DownloadOp { get; set; }
        public DownloadData()
        {
            this.VideoFiles = new List<string>();
            this.DownloadGuid = new List<string>();
            DownloadOp = new List<DownloadOperation>();
        }
    }
}
