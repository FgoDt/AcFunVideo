using AcFunVideo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace AcFunVideo.Class
{
    public class DownloadFunc
    {

        public static string DownloadFileDB = "DFDB.ac";
        public static List<DownloadData> downloadDatas;
        private AcContent _downloadContent;
        public DownloadFunc()
        {
            GetDownloadData();
        }

        public static async Task<List<DownloadData>> GetDownloadData()
        {
            var stream = await LocalStorageFunc.OpenVideoFile(DownloadFileDB);
            if (stream == null)
            {
                downloadDatas = new List<DownloadData>();
                return downloadDatas;
            }

            try
            {
                var sr = new StreamReader(stream).ReadToEnd();
                downloadDatas = JsonConvert.DeserializeObject<List<DownloadData>>(sr);
            }
            catch
            {
                downloadDatas = new List<DownloadData>();
            }

            return downloadDatas;
        }

        public async static void SaveDB()
        {
            try
            {
                var str = JsonConvert.SerializeObject(downloadDatas);
                await LocalStorageFunc.SaveVideoFile(DownloadFileDB, str);
            }
            catch
            {
            }
        }

        public void AddDownload(List<VideoDetail> list)
        {
            foreach (var item in list)
            {
                GetUrl(item);
            }
        }

        private async void GetUrl(VideoDetail content)
        {
            var vsd = new VideoSourceData();
            var url = AcFunAPI.GetVideoSrcUrl(content.VideoId);
            await vsd.GetData(url);
            if (vsd.ListofVideoSourceData == null || vsd.ListofVideoSourceData.Count == 0)
            {
                MSGSHOW("下载失败:\r\n  获取视频地址失败！");
            }

            if (vsd.ListofVideoSourceData.Count > 1)
            {

                bool hasSuper = false;

                foreach (var key in vsd.ListofVideoSourceData.Keys)
                {
                    if (key == "超清")
                    {
                        hasSuper = true;
                    }
                }
                if (hasSuper)
                {
                    DoBackgroundDownload(content, vsd.ListofVideoSourceData["超清"]);
                }
                else
                {
                    DoBackgroundDownload(content, vsd.ListofVideoSourceData["高清"]);
                }

                // MSGSHOW(vsd.ListofVideoSourceData);
            }
            else
            {

                foreach (var dic in vsd.ListofVideoSourceData)
                {
                    DoBackgroundDownload(content,
                        dic.Value);
                    break;
                }
            }

        }

        private async void DoBackgroundDownload(VideoDetail vd, VideosSource vs)
        {
            var downloadData = new DownloadData();

            downloadData.content = vd;
            int i = 0;
            foreach (var item in vs.RealUrl)
            {
                var filename = vd.ACTitle + "_" + vd.Title + "_" + i + ".flv";

                filename = filename.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").
                     Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
                var uri = new Uri(item);
                downloadData.VideoFiles.Add(filename);
                i++;
                //  List<Task> task = new List<Task>();
                try
                {
                    //task.Add(StartOneDownload(filename, uri, downloadData));
                    StartOneDownload(filename, uri, downloadData);
                }
                catch (Exception ex)
                {
                    LocalStorageFunc.DeleteVideoFile(filename);

                    foreach (var dd in downloadDatas)
                    {
                        if (dd.content.DanmakuId == vd.DanmakuId)
                        {
                            downloadDatas.Remove(dd);
                            break;
                        }
                    }
                    MSGSHOW("下载错误：\r\n     " + ex.Message);
                    return;
                }
                finally
                {
                }
            }

            downloadDatas.Add(downloadData);

        }

        private async Task StartOneDownload(string fileName, Uri uri, DownloadData dd)
        {
            StorageFile destinationFile = null;
            try
            {
                var acFolder = await KnownFolders.VideosLibrary.CreateFolderAsync("ACFUNVideo", CreationCollisionOption.OpenIfExists);

                destinationFile = await acFolder.CreateFileAsync(fileName,
                    CreationCollisionOption.ReplaceExisting);
            }
            catch (Exception ex)
            {
                return;
            }

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(uri, destinationFile);
            dd.DownloadGuid.Add(download.Guid.ToString());
            SaveDB();
            download.Priority = BackgroundTransferPriority.Default;
            await download.StartAsync();

        }

        private void MSGSHOW(Dictionary<string, VideosSource> dic)
        {
            var dialog = new ContentDialog()
            {
                Title = "MSG",
                Content = "选择清晰度",
                PrimaryButtonText = "确定",
                // SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };

            // dialog.PrimaryButtonClick += (_s, _e) => { };
            dialog.ShowAsync();
        }


        private void MSGSHOW(string msg)
        {
            var dialog = new ContentDialog()
            {
                Title = "MSG",
                Content = msg,
                PrimaryButtonText = "确定",
                // SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };

            // dialog.PrimaryButtonClick += (_s, _e) => { };
            dialog.ShowAsync();
        }

    }
}
