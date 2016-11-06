using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AcFunVideo.Model;
using AcFunVideo.View;
using AcFunVideo.Class;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace AcFunVideo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CategoryPage : Page
    {

        static ChannelContent _channelContent;
        CategoryPageData _categoryPageData;

        public CategoryPage()
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            SetFlipHeight(e.Size);
        }

        private void SetFlipHeight(Size Size)
        {
            this.CateFlipView.Width = Size.Width;
            CateFlipView.Height = 9 * Size.Width / 34 + 33;//33 is bottom text height
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Content = e.Parameter as ChannelContent;
            if (_channelContent != Content)
            {
                _channelContent = Content;
                CateGoryTitleBox.Text = _channelContent.Title;
                TestFunc();
            }
            //InitFunc();
        }

        private async void TestFunc()
        {
            if (_channelContent != null)
            {
                if (_channelContent.Title != "番剧")
                {
                    _categoryPageData = new CategoryPageData();
                    await _categoryPageData.GetData(AcFunAPI.GetCategoryUrl("11", _channelContent.id));
                    RootCVS.Source = DataCategory();
                    AddRecommendData();
                    AddRankData();
                    AddCategoryButton();
                    AddBanner();
                }
                else
                {

                }
            }
        }

        #region 添加banner
        private void AddBanner()
        {
        List<CategoryBanner> _Bannerlist = new List<CategoryBanner>();
            var temps = _categoryPageData.dicOfData.ToList();
            foreach (var items in temps)
            {
                if (items.Value.Type=="banners")
                {
                    foreach (var item in items.Value.ACContetns)
                    {
                        var cb = new View.CategoryBanner();
                        cb.Tapped += Cb_Tapped;
                        cb.cover.Source = new BitmapImage(new Uri(item.Cover));
                        cb.title.Text = "--ACFUNVIDEO--";
                        cb.items = item;
                        _Bannerlist.Add(cb);
                    }
                }
            
            }
            this.CateFlipView.ItemsSource = _Bannerlist;
        }
        private void Cb_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var cb = sender as CategoryBanner;
            if (cb==null)
            {
                return;
            }

            this.Frame.Navigate(typeof(DetailsPage), cb.items);
        }
        #endregion

        #region 添加分区图标
        private void AddCategoryButton()
        {
            this.CategoryStakPanel.Children.Clear();
            foreach (var item in _categoryPageData.dicOfData.ToList()[0].Value.ACContetns)
            {
                Icon icon = new Icon();
                icon.img.Source = new BitmapImage(new Uri( item.Cover));
                icon.itemSource = item;
                icon.Tapped += Icon_Tapped;
                foreach (var channel in _channelContent.ChildChannels)
                {
                    if (item.ContentId == channel.id)
                    {
                        icon.img.Source = new BitmapImage(new Uri("ms-appx:///" + channel.Cover, UriKind.Absolute));
                        break;
                    }

                }
                icon.categoryTitle.Text = item.Title;
                this.CategoryStakPanel.Children.Add(icon);
            }

        }

        private void Icon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var icon = sender as Icon;
            if (icon!=null)
            {
                this.Frame.Navigate(typeof(SinglePartPage), icon.itemSource);
            }
        }
        #endregion



        private void AddRecommendData()
        {
            if (_categoryPageData.dicOfData.Count < 6)
            {
                RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Collapsed;
                return;
            }
            RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Visible;
            var items = _categoryPageData.dicOfData.ToList();
            if (items[2].Value.Type!="videos")
            {
                RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Collapsed;
                return;
            }
            RecommendListView.ItemsSource = items[2].Value.ACContetns;
        }
        private void AddRankData()
        {
            var items = _categoryPageData.dicOfData.ToList();
            RankListView.ItemsSource = items[1].Value.ACContetns;
        }

        private List<GroupInfoList<object>> DataCategory()
        {

            List<GroupInfoList<object>> categoryData = new List<GroupInfoList<object>>();

            GroupInfoList<object> eachCategory;
            foreach (var single in _categoryPageData.dicOfData)
            {
                eachCategory = new GroupInfoList<object>();
                eachCategory.Key = single.Key;

                if (single.Value.ACContetns == null)
                {
                    continue;
                }

                if (single.Value.Type == "banners")
                {
                    eachCategory.Add(single.Value.ACContetns);
                    categoryData.Add(eachCategory);
                    continue;
                }

                if (single.Value.Type == "channels")
                {
                    eachCategory.Add(single.Value.ACContetns);
                    categoryData.Add(eachCategory);
                    continue;
                }

                foreach (var value in single.Value.ACContetns)
                {
                    if (value.Type=="videos")
                    {
                        eachCategory.Add(value);
                    }
                }
                categoryData.Add(eachCategory);
            }

            return categoryData;
        }

        private void RecommendListView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var NDV = args.ItemContainer.ContentTemplateRoot as NormalDetailView;
            var item = args.Item as AcContent;

            if (NDV != null && item != null)
            {
                var temp = sender.ActualWidth - 38;
                if (temp > 0)
                {
                    NDV.rootGrid.Width = temp;
                }
                NDV.AddData(item);
            }
        }

        private void RecommendListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            AcContent ac = e.ClickedItem as AcContent;
            if (ac != null)
            {
                this.Frame.Navigate(typeof(DetailsPage), ac);
            }
        }

        private void CateFlipView_Loaded(object sender, RoutedEventArgs e)
        {
            var size = Window.Current.Bounds;
            SetFlipHeight(new Size(size.Width,size.Height));
        }
    }
}
