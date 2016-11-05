using AcFunVideo.Class;
using AcFunVideo.Model;
using AcFunVideo.View;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace AcFunVideo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DownloadPage : Page
    {
        private List<DownloadOperation> activeDownloads;
        private CancellationTokenSource cts;

        private List<DownloadData> downloadDatas;
        public DownloadPage()
        {
            cts = new CancellationTokenSource();
            this.InitializeComponent();
        }

        private async void GetDownloadDatas()
        {
        }

        public void Dispose()
        {
            if (cts!=null)
            {
                cts.Dispose();
                cts = null;
            }
            GC.SuppressFinalize(this);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            downloadDatas  = await DownloadFunc.GetDownloadData();
            ShowDownloadFile();
            FindAllDownloadsAsync();
        }

        private async Task FindAllDownloadsAsync()
        {
            activeDownloads = new List<DownloadOperation>();
            IReadOnlyList<DownloadOperation> downloads = null;
            try
            {
                downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            }
            catch ( Exception ex)
            {
                if (!IsExceptionHandled("Discovery error",ex))
                {
                    ACDEBUG.Print(ex.Message);
                }
                return;
            }

            if (downloads.Count>0)
            {
                List<Task> tasks = new List<Task>();
                foreach (var download in downloads)
                {
                    this.activeDownloads.Add(download);
                    tasks.Add(HandleDownload(download, false));
                }
                UpdateDownloadData();

                await Task.WhenAll(tasks);
            }
        }

        private void ShowDownloadFile()
        {
            foreach (var download in downloadDatas)
            {
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    var view = new View.DownlaodView();
                    view.Tag = download.content.DanmakuId;
                    view.titleTB.Text = download.content.ACTitle;
                    view.rootGrid.Height = 100;
                    view.PlayB.Click += PlayB_Click;
                    view.PlayB.Tag = download;
                    view.Item = download;
                    TestSP.Children.Add(view);
                });
            }
        }

        private void PlayB_Click(object sender, RoutedEventArgs e)
        {
            var view = sender as Button;
            if (view!=null)
            {
                this.Frame.Navigate(typeof(DetailsPage), view.Tag);
            }
        }

        private void UpdateDownloadData()
        {
            foreach (var downlaod in downloadDatas)
            {
                foreach (var guid in downlaod.DownloadGuid)
                {
                    foreach (var item in this.activeDownloads )
                    {
                        if (item.Guid.ToString()==guid)
                        {

                            downlaod.DownloadOp.Add(item);
                        }
                    }
                }
            }


            foreach (var download in downloadDatas)
            {
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    var textB = new TextBlock();
                    textB.Tag = download.content.DanmakuId;
                    textB.Text = download.content.ACTitle + ":下载中。。。。";
                    TestSP.Children.Add(textB);
                });
            }

        }

        private void ParseProgress()
        {
            foreach (var item in downloadDatas)
            {
                ulong totalBytes=0;
                ulong receivedBytes=0;
                foreach(var op in item.DownloadOp)
                {
                    totalBytes += op.Progress.TotalBytesToReceive;
                    receivedBytes += op.Progress.BytesReceived;
                }
                if (totalBytes==0&&receivedBytes==0)
                {
                    item.Pre = "失败！请稍后重试";
                    downloadDatas.Remove(item);
                    break;
                }
                else
                item.Pre = (receivedBytes * 100 / totalBytes) + "%";

                foreach (TextBlock tb in TestSP.Children)
                {
                    if (tb.Tag.ToString()==item.content.DanmakuId)
                    {
                        tb.Text = item.content.ACTitle + "：下载->"+item.Pre;
                    }

                }
            }
        }

        private void DownloadProgress(DownloadOperation download)
        {
            BackgroundDownloadProgress currentProgress = download.Progress;

            double percent = 100;
            if (currentProgress.TotalBytesToReceive>0)
            {
                percent = currentProgress.BytesReceived * 100 / currentProgress.TotalBytesToReceive;
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ParseProgress();
                });
                ACDEBUG.Print("下载：" + percent);
            }
        }

        private async Task HandleDownload(DownloadOperation download,bool start)
        {
            try
            {
                activeDownloads.Add(download);
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    await download.StartAsync().AsTask(cts.Token, progressCallback);
                }
                else
                {
                    await download.AttachAsync().AsTask(cts.Token, progressCallback);
                }

                ResponseInformation response = download.GetResponseInformation();

                string statusCode = response != null ? response.StatusCode.ToString() : string.Empty;

            }
            catch ( Exception ex)
            {
            }
            finally
            {
                activeDownloads.Remove(download);
            }
        }

        private bool IsExceptionHandled(string title, Exception ex,DownloadOperation download=null)
        {
            WebErrorStatus error = BackgroundTransferError.GetStatus(ex.HResult);
            if (error == WebErrorStatus.Unknown)
            {
                return false;
            }

            if (download == null)
            {
                ACDEBUG.Print(String.Format(CultureInfo.CurrentCulture, "Error: {0}: {1}", title, error));
            }
            else
            {
                ACDEBUG.Print(String.Format(CultureInfo.CurrentCulture, "Error: {0} - {1}: {2}", download.Guid, title,
                    error));
            }

            return true;
        }
    }
}
