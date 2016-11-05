using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using AcFunVideo.Class;
using AcFunVideo.Model;
using AcFunVideo.View;
using System.Threading.Tasks;
using Windows.Media.Effects;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.System.Display;
using Windows.Storage.Pickers;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace AcFunVideo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private DisplayRequest dispRequest = null;
        public AcContent _SeleCContent;
        public static string did = "";
        private DetailData _detailData;
        private VideoSourceData _videoSourceData;
        private Dictionary<string, VideosSource> _DicOfvideoSource;
        private DispatcherTimer _pageTimer;
        private VideoDetail _selecVideo;
        private DownloadData _localVideoData;
        private DanmuData _DanmuData;
        public DetailsPage()
        {
            this.InitializeComponent();
            _pageTimer = new DispatcherTimer();
            _pageTimer.Interval = new TimeSpan(0, 0, 0, 0, 75);
            _pageTimer.Tick += _pageTimer_Tick;
            _pageTimer.Start();
        }

        private void _pageTimer_Tick(object sender, object e)
        {
            PlayerControlLifeCycle();
            DanmuLifeCycle();
        }
        #region navigation

        bool isLocalFile = false;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            _SeleCContent = e.Parameter as AcContent;
            if (_SeleCContent!=null)
            {
                TestFunc();
            }
            else
            {
                _localVideoData = e.Parameter as DownloadData;
                isLocalFile = true;
                PlayLocalVideo();
            }
            //InitFunc();
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            this.Player.Stop();
            this.Player.Source = null;
            Player.Stop();
            Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Portrait;
        }

        #endregion

        private async Task<bool> InitFunc()
        {
            _detailData = new DetailData();
            var url = AcFunAPI.GetContentUrl(_SeleCContent.ContentId);
            await _detailData.GetData(url);
            _SeleCContent = _detailData.ListOfACContent[0];
            _videoSourceData = new VideoSourceData();
            //url=AcFunAPI.GetVideoSrcUrl(_SeleCContent.DetailVideos[0].)
            //url=AcFunAPI.GetVideoSrcUrl(_SeleCContent.DetailVideos[0].)
            //await _videoSourceData.GetData()
            return true;
        }

        private async void PlayLocalVideo()
        {
            List<string> localUrl = new List<string>();
            foreach (var videoFile in _localVideoData.VideoFiles)
            {
             var file=  await  LocalStorageFunc.GetVideoFileUrl(videoFile);
                if (file!=null)
                {
                    localUrl.Add(file);
                }
            }

            var play_list = new SYEngine.Playlist(SYEngine.PlaylistTypes.LocalFile);
            foreach (var url in localUrl)
            {
                play_list.Append(url, 0, 0);
            }

            Player.Source =await play_list.SaveAndGetFileUriAsync();
            Player.Play();
            Player.MediaOpened += Player_MediaOpened;
            Player.MediaEnded += Player_MediaEnded;
        }

        private async void TestFunc()
        {
            if (_SeleCContent==null)
            {
                return;
            }
            if (_SeleCContent.Bangumi != null)
            {
                GetBangumiVideoData();
            }
            else
            {
                GetDetailData();
            }
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AddComment();
            });
 
            //this.GetDanmuData();
        }

        private async void GetContentData()
        {

        }


        private async Task<bool> AddComment()
        {
            try
            {
                ACCommentData accd = new ACCommentData();
                await accd.GetData(AcFunAPI.GetCommentUrl(_SeleCContent.ContentId,1));
                var sp = accd.GetCommentView(0);
                CommentPivot.Content = sp;
            }
            catch (Exception)
            {
                //todo
                var tb = new TextBlock();
                tb.Text = "评论获取失败！";
                tb.Foreground = ACCOLOR.colorWith(Colors.Black);
                tb.FontSize = 20;
                CommentPivot.Content = tb;
              //  CommentPivot.Visibility = Visibility.Collapsed;
                return false;
            }
            return true;
        }

        //获取弹幕
        private async void GetDanmuData()
        {
            _DanmuData = new DanmuData();
            _DanmuData.DanmaKuId = _selecVideo.DanmakuId;
            var url = AcFunAPI.GetDanmuUrl(_selecVideo.DanmakuId);
            await _DanmuData.GetData(url);
            _DanmuData.ListOfDanmu.Sort((x, y) =>
            {
                return x.RealTime.CompareTo(y.RealTime);
            });
            this.copyDanmuList = _DanmuData.ListOfDanmu;
        }



        private void AddVideoInfoDetail()
        {
            SetVideoPartGridView();
            AddDesc();
        }

        private async void GetDetailData()
        {
          
            DetailData dd = new DetailData();
            var url = AcFunAPI.GetContentUrl(_SeleCContent.ContentId);
            await dd.GetData(url);
            if (dd.ListOfACContent==null||dd.ListOfACContent.Count==0)
            {
                //todo
                MSGSHOW("获取资源失败！");
                return;
            }
            _SeleCContent = dd.ListOfACContent[0];
            _selecVideo = _SeleCContent.DetailVideos[0];
            GetVideoSourceData();
            AddVideoInfoDetail();
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

        private async void GetVideoSourceData()
        {
            if (_videoSourceData == null)
            {
                _videoSourceData = new VideoSourceData();
            }

            if (_selecVideo == null || _selecVideo.VideoId == null)
            {
                return;
            }

            GetDanmuData();
            var url = AcFunAPI.GetVideoSrcUrl(_selecVideo.VideoId);
            await _videoSourceData.GetData(url);
            _DicOfvideoSource = _videoSourceData.ListofVideoSourceData;
            if (_DicOfvideoSource==null|| _DicOfvideoSource.Count == 0)
            {
                //todo error
                MSGSHOW("视频地址解析失败\r\nSRC:"+_selecVideo.SourceType+"--"+_selecVideo.SourceId+"\r\nACID:"+_selecVideo.VideoId);
            }
            AddDescription(_DicOfvideoSource);
            Player.Play();
        }

        private async void GetBangumiVideoData()
        {
            if (_SeleCContent.Bangumi.BangumiId != null)
            {
                var url = AcFunAPI.GetBangumiContentUrl(_SeleCContent.Bangumi.BangumiId);
                BangumiDetailData bdd = new BangumiDetailData();
                await bdd.GetData(url);
                if (bdd.ListOfACContent==null)
                {
                    MSGSHOW("资源获取失败"); 
                    return;
                }
                _SeleCContent = bdd.ListOfACContent[0];
                _selecVideo = _SeleCContent.DetailVideos[0];
                GetVideoSourceData();
                AddVideoInfoDetail();
            }
        }

        private void AddDesc()
        {
            if (_SeleCContent.Title != null)
            {
                TitleBlock.Text = _SeleCContent.Title;
            }
        }

        private void SetTimeControler(double time)
        {
                TimeControler.Maximum = time;
        }

        private void SetVideoPartGridView()
        {
                VideoPartGridView.ItemsSource = _SeleCContent.DetailVideos;
            //if (_SeleCContent.DetailVideos.Count > 1)
            //{
            //    VideoPartGridView.ItemsSource = _SeleCContent.DetailVideos;
            VideoPartGridView.Visibility = Visibility.Visible;
            SortPivot.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    VideoPartGridView.Visibility = Visibility.Collapsed;
            //}
        }

        private void AddDescription(Dictionary<string, VideosSource> dic)
        {
            if (dic == null)
            {
                return;
            }
            DescriptionSelectComboBox.ItemsSource = dic.Keys;
            if (DescriptionSelectComboBox.SelectedIndex == dic.Count - 1)
            {
                var vs = _DicOfvideoSource[DescriptionSelectComboBox.SelectedItem.ToString()];
                PlayVideo(vs);
            }
            else
            {
                DescriptionSelectComboBox.SelectedIndex = dic.Count - 1;
            }
        }

        private void ChangeSize()
        {
            PlayerGrid.Height = Player.Height = PlayerGrid.ActualWidth * 9 / 16;
            TimeControlGrid.Width = TimeControler.Width = PlayerGrid.ActualWidth - 120;

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize();
        }

        bool isXmalSelect = true;
        private void DescriptionSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DescriptionSelectComboBox.SelectedItem != null && !isXmalSelect)
            {
                var vs = _DicOfvideoSource[DescriptionSelectComboBox.SelectedItem.ToString()];
                PlayVideo(vs);
            }
            else if (DescriptionSelectComboBox.SelectedItem != null && isXmalSelect)
            {
                isXmalSelect = false;
            }
        }

        private async void PlayVideo(VideosSource vs)
        {
            var play_list = new SYEngine.Playlist(SYEngine.PlaylistTypes.NetworkHttp);
            foreach (var url in vs.AcUrl)
            {
                play_list.Append(url.url, url.size, (float)url.Time);
            }

            SYEngine.PlaylistNetworkConfigs cfgs = default(SYEngine.PlaylistNetworkConfigs);
            cfgs.HttpUserAgent = "windows 10";
            cfgs.HttpUserAgent = string.Empty;
            cfgs.HttpReferer = string.Empty;
            cfgs.HttpCookie = string.Empty;
            cfgs.UniqueId = string.Empty;
            cfgs.DownloadRetryOnFail = true;
            play_list.NetworkConfigs = cfgs;
            Player.Source = await play_list.SaveAndGetFileUriAsync();
            Player.Play();

            Player.MediaOpened += Player_MediaOpened;
            Player.MediaEnded += Player_MediaEnded;
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (dispRequest != null)
            {
                dispRequest.RequestRelease();
            }
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            var time = Player.NaturalDuration;
            SetTimeControler( time.TimeSpan.TotalSeconds);
            if (dispRequest == null)
            {
                dispRequest = new DisplayRequest();
                dispRequest.RequestActive();

            }
            //throw new NotImplementedException();
        }

        private void PlayVideo(string url)
        {
            Player.Source = new Uri(url);
            Player.Play();
        }





        private void PlayerGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (PlayerTopGrid.Visibility == Visibility.Visible)
            {
                PlayerTopGrid.Visibility = PlayerBotGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                PlayerTopGrid.Visibility = PlayerBotGrid.Visibility = Visibility.Visible;
            }
        }

        private void VideoPartGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            VideoSinglePartView nv = args.ItemContainer.ContentTemplateRoot as VideoSinglePartView;
            if (nv != null)
            {
                var data = args.Item as VideoDetail;
                nv.titleBlock.Text = data.Title;
            }
        }

        private async void VideoPartGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as VideoDetail;

            if (VideoPartGridView.SelectionMode==ListViewSelectionMode.Multiple)
            {
                return;
            }
            if (data != null)
            {
                _selecVideo = data;
                StopMedia();
                GetVideoSourceData();
            }
        }

        private void StopMedia()
        {
            Player.Stop();
            if (Player.Source != null)
            {
                Player.Source = null;
            }
        }

        bool isFullScreen = false;
        private void ChangeFullScreen()
        {
            if (isFullScreen)
            {
                isFullScreen = false;
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Portrait;
                var Bounds = Windows.UI.Xaml.Window.Current.Bounds;
                PlayerGrid.Width = Bounds.Width;
                ChangeSize();
                rootScrollViewer.HorizontalScrollMode = ScrollMode.Auto;
                Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
            }
            else
            {
                isFullScreen = true;
                Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = Windows.Graphics.Display.DisplayOrientations.Landscape;
                var Bounds = Windows.UI.Xaml.Window.Current.Bounds;
                PlayerGrid.Width = Bounds.Width;
                PlayerGrid.Height = Bounds.Height;
                rootScrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                StatusBarTools();
            }
        }


        private void StatusBarTools()
        {
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            //var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            //statusBar.BackgroundColor = Windows.UI.Colors.Orange;
            //statusBar.BackgroundOpacity = 1;
            //statusBar.HideAsync();
        }




        private void FullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeFullScreen();

        }



        private void TimeControler_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (isLifeCycleAutoUpdate)
            {
                isLifeCycleAutoUpdate = false;
                return;
            }
            var sec = e.NewValue;
            Player.Position = new TimeSpan(0, 0, (int)sec);
            if (Player.CurrentState == MediaElementState.Stopped || Player.CurrentState == MediaElementState.Paused)
            {
                Player.Play();
            }
        }

        bool isLifeCycleAutoUpdate;
        private void UpdateTimerControler(int sec)
        {
            TimeControler.Value = sec;
        }

        private void TimeControler_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }


        private void PlayerControlLifeCycle()
        {
            //时间条
            if (TimeControler.Visibility == Visibility.Visible)
            {
                isLifeCycleAutoUpdate = true;
                UpdateTimerControler((int)Player.Position.TotalSeconds);
            }
        }

        #region danmu
        Random rnd = new Random();
        private Vector2 RanPosition()
        {
            double x = rnd.NextDouble() * 300f;
            double y = rnd.NextDouble() * (float)DanmuCanvas.ActualHeight - 20;
            return new Vector2((float)x, (float)y);
        }

        private float RndRadius()
        {
            return (float)rnd.NextDouble() * 150f;
        }

        private byte RndByte()
        {
            return (byte)rnd.Next(256);
        }

        int ss = 1;

        List<DanmuContent> copyDanmuList = new List<DanmuContent>();
        List<bool> onDrawTop = new List<bool>();
        List<bool> onDrawBottom = new List<bool>();
        List<bool> onDrawNormal = new List<bool>();
        private void DanmuLifeCycle()
        {
            var pTime = Player.Position.TotalMilliseconds;
            if (copyDanmuList == null || Player.CurrentState == MediaElementState.Stopped || Player.CurrentState == MediaElementState.Paused)
            {
                return;
            }
            for (int i = 0; i < copyDanmuList.Count; i++)
            {
                if (copyDanmuList[i].RealTime < pTime - 75)
                {
                    //  copyDanmuList.RemoveAt(i);
                    // i--;
                    continue;
                }
                else if (copyDanmuList[i].RealTime > pTime - 75 && copyDanmuList[i].RealTime < pTime)
                {
                    Danmu d = new Danmu();
                    d.color = copyDanmuList[i].RealColor.Color;
                    d.Text = copyDanmuList[i].Msg;
                    d.updateTime = 0;
                    d.location = copyDanmuList[i].RealLocation;
                    if (d.location == DanmuLocation.normal)
                    {
                        var loc = FindOnDrawLocationPosition(d.location);
                        if (loc != -1)
                        {
                            d.position = new Vector2((float)DanmuCanvas.ActualWidth, loc * 22);
                            d.onDrawlocation = loc;
                        }
                        else
                        {
                            d.position = new Vector2((float)DanmuCanvas.ActualWidth, onDrawNormal.Count * 22);
                            d.onDrawlocation = onDrawNormal.Count;
                            if (d.onDrawlocation * 22 > DanmuCanvas.ActualWidth)
                            {
                                return;
                            }
                            onDrawNormal.Add(true);
                        }
                    }
                    else if (d.location == DanmuLocation.bottom)
                    {
                        var loc = FindOnDrawLocationPosition(d.location);
                        if (loc != -1)
                        {
                            d.position = new Vector2(((float)DanmuCanvas.ActualWidth - d.Text.Length * 20) / 2, (float)DanmuCanvas.ActualHeight - (loc + 1) * 22);
                            d.onDrawlocation = loc;
                        }
                        else
                        {
                            d.position = new Vector2(((float)DanmuCanvas.ActualWidth - d.Text.Length * 20) / 2, (float)DanmuCanvas.ActualHeight - (onDrawBottom.Count + 1) * 22);
                            d.onDrawlocation = onDrawBottom.Count;
                            if (d.onDrawlocation * 22 > DanmuCanvas.ActualWidth)
                            {
                                return;
                            }
                            onDrawBottom.Add(true);
                        }

                    }
                    else if (d.location == DanmuLocation.top)
                    {
                        var loc = FindOnDrawLocationPosition(d.location);
                        if (loc != -1)
                        {
                            d.position = new Vector2(((float)DanmuCanvas.ActualWidth - d.Text.Length * 20) / 2, loc * 22);
                            d.onDrawlocation = loc;
                        }
                        else
                        {
                            d.position = new Vector2(((float)DanmuCanvas.ActualWidth - d.Text.Length * 20) / 2, onDrawTop.Count * 22);
                            d.onDrawlocation = onDrawTop.Count;
                            if (d.onDrawlocation * 22 > DanmuCanvas.ActualWidth)
                            {
                                return;
                            }
                            onDrawTop.Add(true);
                        }
                    }
                    d.isLocationKey = true;
                    onDrawList.Add(d);
                }
                else if (copyDanmuList[i].RealTime > pTime)
                {
                    break;
                }
            }
        }

        private int FindOnDrawLocationPosition(DanmuLocation location)
        {
            switch (location)
            {
                case DanmuLocation.normal:
                    for (int i = 0; i < onDrawNormal.Count; i++)
                    {
                        if (onDrawNormal[i] == false)
                        {
                            onDrawNormal[i] = true;
                            return i;
                        }
                    }
                    return -1;
                case DanmuLocation.top:
                    for (int i = 0; i < onDrawTop.Count; i++)
                    {
                        if (onDrawTop[i] == false)
                        {
                            onDrawTop[i] = true;
                            return i;
                        }
                    }
                    return -1;
                case DanmuLocation.bottom:
                    for (int i = 0; i < onDrawBottom.Count; i++)
                    {
                        if (onDrawBottom[i] == false)
                        {
                            onDrawBottom[i] = true;
                            return i;
                        }
                    }
                    return -1;
                default:
                    return -1;
            }
        }

        List<Danmu> onDrawList = new List<Danmu>();

        Danmu dmu = new Danmu();
        private void DanmuCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            cl.Dispose();
            cl = new CanvasCommandList(sender);

            using (var clds = cl.CreateDrawingSession())
            {
                for (int i = 0; i < onDrawList.Count; i++)
                {
                    clds.DrawText(onDrawList[i].Text, onDrawList[i].position.X + 2, onDrawList[i].position.Y + 1, Colors.Black);
                    clds.DrawText(onDrawList[i].Text, onDrawList[i].position, onDrawList[i].color);
                }

            }

            args.DrawingSession.DrawImage(cl);
        }


        CanvasCommandList cl;

        private void DanmuCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            cl = new CanvasCommandList(sender);
        }


        private void DanmuCanvas_Update(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs args)
        {

            double width = sender.Size.Width;

            for (int i = 0; i < onDrawList.Count; i++)
            {
                Vector2 v2;

                dmu = onDrawList[i];
                if (dmu.location == DanmuLocation.normal)
                {
                    if (onDrawList[i].position.X < -(onDrawList[i].Text.Length * 23))
                    {
                        onDrawList.RemoveAt(i); i--;
                        continue;
                    }
                    if (dmu.isLocationKey && dmu.position.X < width - dmu.Text.Length * 22)
                    {
                        onDrawNormal[dmu.onDrawlocation] = false;
                        onDrawList[i].isLocationKey = false;
                    }
                    v2 = new Vector2(onDrawList[i].position.X - ss * 3, onDrawList[i].position.Y);
                    //   clds.DrawText(onDrawList[i].Text, v2.X + 2, v2.Y + 1, Colors.Black);
                    //clds.DrawText(onDrawList[i].Text, v2, onDrawList[i].color);
                    onDrawList[i].position = v2;
                }
                else
                if (dmu.location == DanmuLocation.top)
                {
                    if (dmu.isLocationKey && dmu.updateTime >= 200)
                    {
                        if (onDrawTop.Count <= 0)
                        {
                            return;
                        }
                        dmu.isLocationKey = false;
                        onDrawTop[dmu.onDrawlocation] = false;
                        onDrawList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        //clds.DrawText(onDrawList[i].Text, onDrawList[i].position.X + 2, onDrawList[i].position.Y + 1, Colors.Black);
                        //clds.DrawText(onDrawList[i].Text, onDrawList[i].position, onDrawList[i].color);
                        onDrawList[i].updateTime++;
                    }

                }
                else if (dmu.location == DanmuLocation.bottom)
                {
                    if (dmu.isLocationKey && dmu.updateTime >= 200)
                    {
                        if (onDrawBottom.Count <= 0)
                        {
                            return;
                        }
                        dmu.isLocationKey = false;
                        onDrawBottom[dmu.onDrawlocation] = false;
                        onDrawList.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        //clds.DrawText(onDrawList[i].Text, onDrawList[i].position.X + 2, onDrawList[i].position.Y + 1, Colors.Black);
                        //clds.DrawText(onDrawList[i].Text, onDrawList[i].position, onDrawList[i].color);
                        onDrawList[i].updateTime++;
                    }
                }
            }
        }

        #endregion

        private void Download_Click(object sender, RoutedEventArgs e)
        {

            var list = new List<VideoDetail>();
            if (VideoPartGridView.SelectionMode != ListViewSelectionMode.Multiple)
            {
                _selecVideo.ACTitle = _SeleCContent.Title;
                list.Add(_selecVideo);
            }
            else
            {
                var lis = VideoPartGridView.SelectedItems;
                foreach (var item in lis)
                {
                    var jj = item as VideoDetail;
                    jj.ACTitle = jj.Title;
                    list.Add(jj);
                }
            }
            Class.DownloadFunc df = new DownloadFunc();
            df.AddDownload(list);
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            DetailPivot.SelectedIndex = 1;

            //set list view select mode
            VideoPartGridView.SelectionMode = ListViewSelectionMode.Multiple;
        }

        private void GoDownload_Click(object sender, RoutedEventArgs e)
        {
            this.Player.Stop();
            this.Player.Source = null;
            this.Frame.Navigate(typeof(DownloadPage));
        }
    }



    public class Danmu
    {
        public Vector2 position { get; set; }
        public Color color { get; set; }
        public string Text { get; set; }
        public int updateTime { get; set; }
        public DanmuLocation location { get; set; }
        public int onDrawlocation { get; set; }
        public bool isLocationKey { get; set; }
    }

}
