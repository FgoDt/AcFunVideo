using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AcFunVideo.Class;
using AcFunVideo.Model;
using AcFunVideo.Utilites;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AcFunVideo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page,INotifyPropertyChanged
    {
        private string keywork = "";
        private SearchControl searchControl;
        private int pageNo = 0;
        private int pageSize = 20;
        private int TotalCount = 0;
        public SearchPage()
        {
            this.InitializeComponent();
            DataContext = this;  
            NavigationCacheMode = NavigationCacheMode.Enabled;
            GetHotKey();
        }

        private IncrementalLoadingCollection<AcContent> _contents;

        public IncrementalLoadingCollection<AcContent> Contents
        {
            get { return _contents; }
            set
            {
                _contents = value;
                OnPropertyChanged();
            }
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationCacheMode=NavigationCacheMode.Disabled;
            }
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is string)
            {
                keywork = (string) e.Parameter;
            }
            else
            {
                SuggestBox.Focus(FocusState.Keyboard);
            }
            //SystemNavigationManager.GetForCurrentView().BackRequested += SearchPage_BackRequested;
            base.OnNavigatedTo(e);
        }

        //private void SearchPage_BackRequested(object sender, BackRequestedEventArgs e)
        //{
            
        //}

        private async void SuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(sender.Text))
            {
                var suggests = await GetSuggest(sender.Text);
                sender.ItemsSource = suggests;
            }
        }

        private void SuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var item = args.SelectedItem as Suggest;
            keywork = item.Name;
            Search();
        }

        private void SuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            keywork = SuggestBox.Text;
            Search();
        }

        private void SuggestBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SuggestBox.Text))
                {
                    keywork = SuggestBox.Text;
                    Search();
                }
            }
        }

        private async Task<List<Suggest>> GetSuggest(string key)
        {
            try
            {
                HttpEngine engine=new HttpEngine();
                var str = await engine.GetString("http://search.app.acfun.cn/suggest?q=" + key);
                var suggests = JsonConvert.DeserializeObject<RespData<List<Suggest>>>(str);
                return suggests.Data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[AcFunVideo][SearchPage.GetSuggest]" + ex.Message);
                return null;
            }
        }

        private async void GetHotKey()
        {
            try
            {
                HttpEngine engine=new HttpEngine();
                var str = await engine.GetString("http://api.aixifan.com/hotwords");
                var keys = JsonConvert.DeserializeObject<RespData<List<Hotword>>>(str);
                var items = keys.Data.Select(x => new Suggest() {Name = x.Value}).ToList();
                SuggestBox.ItemsSource = items;
                SuggestBox.IsSuggestionListOpen = true;
            }
            catch (Exception)
            {
                
            }
        }
        private async void Search()
        {
            if (searchUrlData == null) return; 
             pageNo = 0;
            pageSize = 20;
            TotalCount = 0;
            Contents=new IncrementalLoadingCollection<AcContent>(OnGetData);
        }

        private async Task<Tuple<List<AcContent>, bool>> OnGetData(uint arg)
        {
            var items = await GetSearchData();
            if (items != null)
            {
                bool canNext = Contents.Count < TotalCount;
                if (searchUrlData.ContentType == AcContentType.User)
                    canNext = false;
                return new Tuple<List<AcContent>, bool>(items,canNext);
            }
            return new Tuple<List<AcContent>, bool>(new List<AcContent>(), false);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SearchTypeEx_OnClick(object sender, RoutedEventArgs e)
        {
            if (searchControl == null)
            {
                var point = SuggestBox.TransformToVisual(this).TransformPoint(new Point());
                searchControl =new SearchControl(point.Y+ SuggestBox.ActualHeight, OnGetSearchParam);
            }
                
             searchControl.Show();
        }

        private SearchUrlData searchUrlData = new SearchUrlData
        {
            ContentType = AcContentType.Videos,
            Url = "http://search.app.acfun.cn/search?sortField=score&type=2&aiCount=0&spCount=0&fqChannelId=63"
        };
        private void OnGetSearchParam(SearchUrlData data)
        {
            if(searchUrlData.Url!= data.Url)
            {
                searchUrlData = data;
                Search();
            } 
        }

        private async Task<List<AcContent>> GetSearchData()
        {
            try
            {
                var url = searchUrlData.Url + "&q=" + keywork;
                switch (searchUrlData.ContentType)
                {
                    case AcContentType.User:
                        url += "&pageNo=0&pageSize=0";
                        break;
                    default:
                        pageNo++;
                        url += "&pageNo=" + pageNo + "&pageSize=" + pageSize;
                        break;
                }
                HttpEngine engine = new HttpEngine();
                var str = await engine.GetString(url);
                var jsons = JsonConvert.DeserializeObject<RespData<RespPage<RespList<List<AcContent>>>>>(str); 
                if (searchUrlData.ContentType== AcContentType.User)
                {
                    if (jsons.Data.Page.User != null)
                    {
                        jsons.Data.Page.List = jsons.Data.Page.User;
                    }
                }
                TotalCount = jsons.Data.Page.TotalCount;
                List<AcContent> lists=new List<AcContent>();
                foreach (var c in jsons.Data.Page.List)
                {
                    c.AcContentType = searchUrlData.ContentType;
                    lists.Add(c);
                }
                
                return lists;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (AcContent) e.ClickedItem;
            if (item.AcContentType == AcContentType.Videos)
            {
                Frame.Navigate(typeof(DetailsPage), item);
            }
            else if (item.AcContentType == AcContentType.Bangumis)
            {
                Bangumi bangumi=new Bangumi();
                bangumi.Title = item.Title;
                bangumi.BangumiId = item.id;
                item.Bangumi = bangumi;
                Frame.Navigate(typeof(DetailsPage), item);
            }
            else
            {
                this.Frame.Navigate(typeof(AboutPage));
            }
        }
    }
}
