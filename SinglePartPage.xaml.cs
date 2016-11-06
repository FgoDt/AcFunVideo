using AcFunVideo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace AcFunVideo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SinglePartPage : Page
    {
        private string _sort = "0";
        private long _stimer = 0;
        private long _etimer = 0;
        private string _channelid ="";
        private int _page = 1;
        Model.SinglePartDatas _singlePartDatas;
        public SinglePartPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.InitializeComponent();
            AddSwitchButton();

        }
        private void AddSwitchButton()
        {
            SortButton.defaultButton.title.Text = "最新";
            SortButton.AddButton("播放");
            SortButton.AddButton("评论");
            SortButton.AddButton("弹幕");
        }
        private void TimePicker_Tapped(object sender, TappedRoutedEventArgs e)
        {

            this.timePicker.Visibility = Visibility.Visible;
            var textblock = sender as TextBlock;
            if (textblock == null)
                return;

            if (textblock.Name == "STextBlock")
            {
                isStSelct = true;
                // var times = STextBlock.Text.Split('/');
            //    var t = new DateTimeOffset(new DateTime((long)_stimer));
            //    timePicker.Date = t;
              
            }
            else
            {
                isStSelct = false;
            }
        }


        private void timePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.timePicker.Visibility = Visibility.Collapsed;
            if (isStSelct)
            {
                var dateoffset = e.NewDate;
                _stimer = Class.Utils.GetTimeToUNIX(dateoffset.DateTime)*1000;
                _etimer = Class.Utils.GetTimeToUNIX(dateoffset.DateTime.AddMonths(1)) * 1000;
                STextBlock.Text = e.NewDate.Year.ToString() + "/" + e.NewDate.Month + "/" + e.NewDate.Day;
                var en = e.NewDate.AddMonths(1);
                ETextBlock.Text = en.Year + "/" + en.Month  + "/" + en.Day;
            }
            else
            {
                _etimer = Class.Utils.GetTimeToUNIX(e.NewDate.DateTime)*1000;
                _stimer = Class.Utils.GetTimeToUNIX(e.NewDate.DateTime.AddMonths(-1))*1000;
                var st = e.NewDate.AddMonths(-1);
                STextBlock.Text = st.Year.ToString() + "/" + st.Month  + "/" + st.Day;
                ETextBlock.Text = e.NewDate.Year + "/" + e.NewDate.Month  + "/" + e.NewDate.Day;
            }
          
            GetData();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var ac = e.Parameter as Model.AcContent;
            if (ac != null)
            {
                _channelid = ac.ContentId;
                GetInitData();
                TitleBolock.Text = ac.Title;
            }
        }


        private async void GetInitData()
        {
            var endTime = DateTime.UtcNow;
            var startTime = DateTime.UtcNow.AddMonths(-1);
            STextBlock.Text = startTime.Year.ToString() + "/" + startTime.Month + "/" + startTime.Day;
            ETextBlock.Text = endTime.Year + "/" + endTime.Month + "/" + endTime.Day;
            _etimer = Class.Utils.GetTimeToUNIX(endTime)*1000;
            _stimer = Class.Utils.GetTimeToUNIX(startTime)*1000;
            GetData();
        }

        private async void GetData()
        {
            RootListView.ItemsSource = null;
             var url = Class.AcFunAPI.GetSinglePartDataUrl(_sort, _page, _channelid, _stimer.ToString(), _etimer.ToString());
            //if (_singlePartDatas!=null)
            //{
            //    RootListView.Items.Clear();
            //}
            _singlePartDatas = new Model.SinglePartDatas();
            await _singlePartDatas.GetData(url);
            RootListView.ItemsSource = _singlePartDatas.CollectionOfACContent;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        bool isStSelct = false;
        private void pickTime_Click(object sender, RoutedEventArgs e)
        {
          
            if (TimeStackPanel.Visibility == Visibility.Collapsed)
            {
                this.TimeStackPanel.Visibility = Visibility.Visible;
            }
            else
                this.TimeStackPanel.Visibility = Visibility.Collapsed;
        }

        private void RootListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var item = args.Item as Model.AcContent;
            var view = args.ItemContainer.ContentTemplateRoot as View.NormalDetailView;
            view.Width = RootListView.Width ;
            view.rootGrid.Width = view.Width - 20;
            if (item != null && view != null)
            {
                view.AddData(item);
            }

            Model.AcContent last;
            if (_singlePartDatas.CollectionOfACContent.Count>4)
            {
                last = _singlePartDatas.CollectionOfACContent[_singlePartDatas.CollectionOfACContent.Count - 4] as AcContent;
            }
            else
            {
                if (_singlePartDatas.CollectionOfACContent.Count>0)
                {
                    last = _singlePartDatas.CollectionOfACContent[_singlePartDatas.CollectionOfACContent.Count - 1] as AcContent;
                }
                else
                last = null;
            }

            if (last!=null&& item.ContentId==last.ContentId)
            {
                AddMore();
            }

        }

        private async Task  AddMore()
        {
            _page++;
            var url = Class.AcFunAPI.GetSinglePartDataUrl(_sort, _page, _channelid, _stimer.ToString(), _etimer.ToString());
            await _singlePartDatas.GetData(url);
        }

        private void RootListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ac = e.ClickedItem as Model.AcContent;

            if (ac != null)
            {
                this.Frame.Navigate(typeof(DetailsPage), ac);
            }
        }

        private void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void UnderLineSwitchButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var button = sender as View.UnderLineSwitchButton;
            if (button!=null)
            {
                _sort = button.SelectIndex.ToString();
            }
            this.GetData();
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TimeStackPanel.Visibility == Visibility.Collapsed)
            {
                this.TimeStackPanel.Visibility = Visibility.Visible;
            }
            else
                this.TimeStackPanel.Visibility = Visibility.Collapsed;
        }

        private void RootListView_Loaded(object sender, RoutedEventArgs e)
        {
            this.RootListView.Width = Window.Current.Bounds.Width;
        }
    }
}
