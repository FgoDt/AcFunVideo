//////////
///所有功能需要重构
///
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AcFunVideo.Class;
using Windows.UI.ViewManagement;
using Windows.Storage.Streams;
using Windows.Data.Html;
using Windows.Data.Json;
using System.Threading.Tasks;
using AcFunVideo.Model;
using Windows.UI.Xaml.Media.Imaging;
using AcFunVideo.View;
using System.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.UI.Core;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace AcFunVideo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Rect DiveRect;
        Dictionary<string, AXFContent> _dic;
        MainPageFunc _MPF ;
        List<AcContent> BananaData;
        List<FDImgBox> _FDIMGS = new List<FDImgBox>();
        ACChannelsData _ACChannelsData;
        public static MainPage kCurrent;

        public MainPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
            PageSetting();
            this.Loaded += MainPage_Loaded;
            InitData();
            kCurrent = this;
        }
        

        private async void Test()
        {

            //Encode("sdj", "zx26mfbsuebv72ja");
            //Encode("44616713980388605ab4d_030020010057150762B6552D9B7D2F302AAB41-D431-EDEE-DF77-5A4E5E993D74_8466", "zx26mfbsuebv72ja");
            var md5str = Utils.GetMD5String("GET:/common/partner/play:1462284491:78554907b127c3853f8e956243dc74c4");
            var url = "http://acfun.api.mobile.youku.com/common/partner/play?_t_=1462284491&e=md5&_s_=" + md5str
                + "&point=1&id=CMzQwMTA3Mg==&format=1,5,6,7,8&language=guoyu&did=a721b02c70aa0c2784afa3a39b9356b6&ctype=87&audiolang=1&pid=528a34396e9040f3";
            HttpEngine he = new HttpEngine();
            var data = await he.Get(url);

            var reader = new StreamReader(data.AsStreamForRead());
            var str = reader.ReadToEnd();
            var obj= Newtonsoft.Json.Linq.JObject.Parse(str);
            var keystr = obj["data"].ToString();
            var destr = Decode(keystr, Aeskey);

            //http://k.youku.com/player/getFlvPath/sid/44616713980388605ab4d_00/st/mp4/fileid/030020010057150762B6552D9B7D2F302AAB41-D431-EDEE-DF77-5A4E5E993D74/?K=8aae87b234a1cb02282b5e1a&hd=1&myp=0&ts=1519.867&ypp=0&ep=C55ccxIEzL9JF0%2FF5gjcoEbkDHaOmE%2FI6YrmN90c%2FieiMqDvcFpZvjPJK9ojN%2BP0ZzFvykhvKduhPiCXpaaWGpUPQos0wjsPI%2BH6PVV%2FwXCORi1awYC9q1DYqq7W&ctype=86&ev=1&token=8466&oip=3550665720
        }

        const string Aeskey = "qwer3as2jin4fdsa";
        private string Decode(string value ,string key)
        {
            var buffkey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(value);
            SymmetricKeyAlgorithmProvider aes =
                SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcb);

            // Create a symmetric key.
            var symetricKey = aes.CreateSymmetricKey(buffkey);
            var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);

            string strDecrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);
            return strDecrypted;

        }



        private void Encode(string value,string key)
        {
            var bufKey = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcb);
            IBuffer enBuffer;
            var  data = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);
            enBuffer = data;
            var symtricKey = aes.CreateSymmetricKey(bufKey);
            IBuffer enoder = CryptographicEngine.Encrypt(symtricKey, enBuffer, null);

            string strEncrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, enoder);
        }

        private async void GetAllChannel()
        {
             _ACChannelsData = new ACChannelsData();
            _ACChannelsData.GetLocalData();
        }

        private async void InitData()
        {
            HomeData hd = new HomeData();
           _dic=await hd.GetData();
            AddMainBanner();
            AddSpecailImg();
            AddToday();
            AddBananaRank();
            AddBangumi();
            AddMainPageNormalView();
            AddCategooryIcon();
            AddNewBangumiPage();
            GetAllChannel();
        }

        private async Task<int> AddNewBangumiPage()
        {
            var url = AcFunAPI.GetHomeBangumiUrl();
            NewBangumiData nbd = new NewBangumiData();
            await nbd.GetData(url);
            NewBangumiGridView.ItemsSource = nbd.ListOfACContent;
            return 0;
        }


        private void AddMainBanner()
        {
            try
            {
                if (_dic.Count<=0)
                {
                    return;
                }
                MainPageFunc.BannerFunc( MainFlipView, _dic["轮播图"].Contents);
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }

        private async void AddSpecailImg()
        {
          
            try
            {
                if (_dic.Count <= 0)
                {
                    return;
                }
                _dic["二次元日历"] = await GetSingleDicData("二次元日历");
                CalendarImg.Source = new BitmapImage(new Uri(_dic["二次元日历"].Contents[0].Cover));
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }

        private async void AddToday()
        {
           
            try
            {
                if (_dic.Count <= 0)
                {
                    return;
                }
                _dic["活动banner"] = await GetSingleDicData("活动banner");
                TodyImg.Source = new BitmapImage(new Uri(_dic["活动banner"].Contents[0].Cover));
            }
            catch (Exception ex)
            {
                ACDEBUG.Print(ex.Message);
            }
        }

         private async void AddBangumi()
        {
            if (_dic.Count<=0)
            {
                return;
            }
            BangumiData bd = new BangumiData();
            _dic["番剧"].Contents =await bd.GetData(AcFunAPI.GetRegionUrlById(_dic["番剧"].Id));
            BangumiRankView.ItemsSource = _dic["番剧"].Contents;
        }

        private async void AddBananaRank()
        {

            if (_dic.Count <= 0)
            {
                return;
            }
            _dic["香蕉排行榜"] = await GetSingleDicData("香蕉排行榜");
            BananaRankData brd = new BananaRankData();
           var  brdurl = AcFunAPI.GetBananaRecommendUrl();
           var listData =await  brd.GetData(brdurl);
            if (listData.Count>4)
            {
                BananaData = listData;
                List<AcContent> temp = new List<AcContent>();
                for (int i = 0; i < 4; i++)
                {
                    var data = listData[i];
                    temp.Add(data);
                }
                BananaRankView.ItemsSource = temp;
            }
            else
            {
                BananaRankView.ItemsSource = listData;
            }
        }


        private async void AddMainPageNormalView()
        {
            AddView(RecommentView, "热门推荐");
            AddView(FunRankView, "娱乐");
            AddView(GameView, "游戏");
            AddView(CartoonRankView, "动画");
            AddView(VideoView, "影视");
            AddView(MusicView, "音乐");
            AddView(DanceView, "舞蹈");
            AddView(TechView, "科技");
            AddView(SportView, "体育");
            AddView(GirlView, "彼女");
            AddView(BoyView, "鱼塘");

        }

        private async void AddCategooryIcon()
        {
            MainPageFunc.CategoryFunc(CategoryView);
        }

        private  async void AddView( GridView gv,string key )
        {
            
            if (_dic.Count<=0)
            {
                return;
            }
            var ax = await GetSingleDicData(key);
            gv.ItemsSource = ax.Contents;
        }

        private async Task<AXFContent> GetSingleDicData(string key)
        {
            HomeData hd = new HomeData();
           _dic[key] =  await hd.GetSingleData((_dic[key].Id), key);
            return _dic[key];
        }

       
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ApplicationView av = ApplicationView.GetForCurrentView();
            av.SetPreferredMinSize(new Size(300, 800));
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            SetDiveRect();
        }

        void PageSetting()
        {
            this.Background = ACCOLOR.ACDEEPBLU;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            SizeChange();
        }

        private void SizeChange()
        {
            //  System.Diagnostics.Debug.WriteLine(MainGrid.Width);
            MainFlipView.Height = MainFlipView.ActualWidth * 9 / 16;
            foreach (var item in SectionStackPanel.Children)
            {
                Grid temp = item as Grid;
                if (temp != null)
                {
                    var Left = (MainFlipView.ActualWidth - temp.ActualWidth * 4) / 8;
                    temp.Margin = new Thickness(Left, 0, Left, 0);
                }
            }
            if (isPartPivotLoad)
            {
                ReSizeTopBar();
            }
         
        }

        private void TodyTextBlock_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton clickButton = sender as HyperlinkButton;
            AcFunVideo.Model.AcContent ac = clickButton.Tag as AcFunVideo.Model.AcContent;
            
        }

        private void TodyImg_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void SetDiveRect()
        {
            DiveRect = Window.Current.Bounds;
        }

        private void NormalView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            NormalView nv = args.ItemContainer.ContentTemplateRoot as NormalView;
            if (nv!=null)
            {
                if (DiveRect==null||DiveRect.Width<=0)
                {
                    SetDiveRect();
                }
                nv.Width = DiveRect.Width / 2 - 20;
                nv.Height = nv.Width - 10;
                nv.Margin = new Thickness(10, 0, 5, 0);
                nv.AddAllData(args.Item as AcContent);
                bool has = false;
                for (int i = 0; i < _FDIMGS.Count; i++)
                {
                    if (_FDIMGS[i]==nv.img)
                    {
                        has = true;
                    }
                }
                if (!has)
                {
                    _FDIMGS.Add(nv.img);
                }
            }
        }

        private void BangumiRankView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
             CartoonView nv = args.ItemContainer.ContentTemplateRoot as CartoonView;
            if (nv != null)
            {
                if (DiveRect == null || DiveRect.Width <= 0)
                {
                    SetDiveRect();
                }
                nv.Width = DiveRect.Width / 3 - 25;
                nv.Height = nv.Width * 6 / 3;
                nv.Margin = new Thickness(10, 0, 5, 0);
                nv.AddData(args.Item as AcContent);
            }
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            UnloadGIFView();
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    if (isPartPivotLoad)
                    {
                        ReSizeTopBar();
                    }
                    break;
                default:
                    break;
            }

        }

        private void UnloadGIFView()
        {
            if (MainPivot.SelectedIndex != 0)
            {
                foreach (var item in _FDIMGS)
                {
                    item.UNLOAD();
                }
            }
            else
            {
                foreach (var item in _FDIMGS)
                {
                    item.LOAD();
                }
            }
        }

        private void PvoitTitleMain_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var clickObj = sender as TextBlock;

            if (clickObj!=null)
            {
                switch (clickObj.Text)
                {
                    case "首页":
                        MainPivot.SelectedIndex = 0;
                        break;
                    case "新番":
                        MainPivot.SelectedIndex = 1;
                        break;
                    default:
                        MainPivot.SelectedIndex = 2;
                        ReSizeTopBar();
                        break;
                }
            }
        }

        bool isPartPivotLoad = false;
        private void ReSizeTopBar()
        {
            foreach (var item in PTopStackPanel.Children)
            {
                Grid sp = item as Grid;
                sp.UpdateLayout();
                if (sp != null)
                {
                    var left = (PartPiovtItem.ActualWidth-20 - sp.ActualWidth * 4) / 9.5;
                    sp.Margin = new Thickness(left, 0, left, 0);
                }
            }
        }

        private void PTopStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            isPartPivotLoad = true;
            ReSizeTopBar();
        }

        private void CategoryView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            CategoryIcon nv = args.ItemContainer.ContentTemplateRoot as CategoryIcon;
            if (nv != null)
            {
                nv.AddData(args.Item as MainPageCategoryData);

                if (4*80>(DiveRect.Width-32))
                {
                  nv.Width = nv.Height = (DiveRect.Width- 32) / 4;
                  nv.RootGrid.Width = nv.RootGrid.Height = nv.Width - 2;
                    nv.IMG.Width = nv.IMG.Height = nv.IMG.Width - 5;
                    nv.RootGrid.Margin = new Thickness(0, 2, 0, -2);
                    nv.Title.FontSize = 13;
                    nv.Title.Margin = new Thickness(0, 0, 5, 0);
                }
            }
        }

        private void NewBangumiGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {

             CartoonView nv = args.ItemContainer.ContentTemplateRoot as CartoonView;
            if (!CateisTV)
            {

                //var s = NewBangumiGridView.ItemTemplate;
                if (nv != null)
                {
                    if (DiveRect == null || DiveRect.Width <= 0)
                    {
                        SetDiveRect();
                    }
                    nv.Width = DiveRect.Width / 3 - 25;
                    nv.Height = nv.Width * 6 / 3;
                    nv.Margin = new Thickness(10, 0, 5, 0);
                    nv.AddData(args.Item as AcContent);
                }
            }
            else
            {
                var nov = args.ItemContainer.ContentTemplateRoot as CartoonView;
                if (nov != null)
                {
                    if (DiveRect == null || DiveRect.Width <= 0)
                    {
                        SetDiveRect();
                    }
                    nov.Width = DiveRect.Width / 3 - 25;
                    nov.Height = nov.Width + 20;
                    nov.Margin = new Thickness(10, 0, 5, 0);
                    //nov.AddAllData(args.Item as AcContent);
                    nov.AddTVData(args.Item as AcContent);
                }
            }
        }



        private string CateTimeSelect = "N";
        private bool CateisTV = false;

        private string GetWeekTime()
        {
            switch (CateTimeSelect)
            {
                case "N":
                    return "0";
                case "一":
                    return "1";
                case "二":
                    return "2";
                case "三":
                    return "3";
                case "四":
                    return "4";
                case "五":
                    return "5";
                case "六":
                    return "6";
                case "日":
                    return "7";
                default:
                    break;
            }
            return "0";
        }
        private async void CategorySelec()
        {
            string url;
            NewBangumiData nbd = new NewBangumiData();
            if (CateisTV)
            {
                url = AcFunAPI.GetHomenominateUrl(GetWeekTime());
                nbd.isTV = true;
            }
            else
            {
                url = AcFunAPI.GetHomeBangumiUrl(GetWeekTime());
            }

           
            await nbd.GetData(url);
            NewBangumiGridView.ItemsSource = nbd.ListOfACContent;
        }
        
        private async void Category_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            string SelectText = "";
            if (grid!=null)
            {
                foreach (Grid item in Category.Children)
                {
                    if (item!=grid)
                    {
                        item.BorderThickness = new Thickness(0);
                        var temp = item.Children[0] as TextBlock;
                        temp.Foreground = ACCOLOR.colorWith(Colors.Black);
                    }
                    else
                    {
                        item.BorderThickness = new Thickness(1);
                        var temp = item.Children[0] as TextBlock;
                        temp.Foreground = ACCOLOR.ACDEEPBLU;
                        SelectText = temp.Text;
                    }
                }
            }

            if (SelectText=="剧集")
            {
                CateisTV = true;
            }
            else
            {
                CateisTV = false;
                var url = AcFunAPI.GetHomeBangumiUrl();
                NewBangumiData nbd = new NewBangumiData();
                await nbd.GetData(url);
                NewBangumiGridView.ItemsSource = nbd.ListOfACContent;
            }

            CategorySelec();
        }

        private void CategoryPart_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = sender as Grid;
            if (grid != null)
            {
                foreach (Grid item in CategoryPart.Children)
                {
                    if (item != grid)
                    {
                        item.BorderThickness = new Thickness(0);
                        var temp = item.Children[0] as TextBlock;
                        temp.Foreground = ACCOLOR.colorWith(Colors.Black);
                    }
                    else
                    {
                        item.BorderThickness = new Thickness(1);
                        var temp = item.Children[0] as TextBlock;
                        temp.Foreground = ACCOLOR.ACDEEPBLU;
                        CateTimeSelect = temp.Text;
                    }
                }
            }
            CategorySelec();
        }

        private void NavigationToDetailsPage(AcContent ac)
        {
            this.Frame.Navigate(typeof(DetailsPage),ac);
        }

        private void MainFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

 

        private void MainGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            AcContent ac = e.ClickedItem as AcContent;
            if (ac!=null)
            {
                NavigationToDetailsPage(ac);
            }
        }

        private void CategoryView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as MainPageCategoryData;
            if (data.title=="下载")
            {
                this.Frame.Navigate(typeof(DownloadPage));
                return;
            }
            if (data!=null&&_ACChannelsData!=null)
            {

                foreach (var key in _ACChannelsData.dicOfData.Keys)
                {
                    if (key==data.title)
                    {
                        var cc = _ACChannelsData.dicOfData[key];
                        this.Frame.Navigate(typeof(CategoryPage),cc);
                        break;
                    }
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += SystemNavigationManager_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        DateTime preTimeToBack;
        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (preTimeToBack==new DateTime())
            {
                preTimeToBack = DateTime.Now;
            }
            else if ((DateTime.Now-preTimeToBack).TotalMilliseconds<600)
            {
                preTimeToBack = DateTime.Now;
                return;
            }
            if ( this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
                
            }

            preTimeToBack = DateTime.Now;
        }

        private void MainSearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }
    }
}
