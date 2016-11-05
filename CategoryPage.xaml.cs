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
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Content = e.Parameter as ChannelContent;
            if (_channelContent != Content)
            {
                _channelContent = Content;
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

        private void AddBanner()
        {
            List<CategoryBanner> list = new List<CategoryBanner>();
            foreach (var item in _categoryPageData.dicOfData.ToList()[0].Value.ACContetns)
            {
                var cb = new View.CategoryBanner();
                cb.Tapped += Cb_Tapped;
                cb.cover.Source = new BitmapImage(new Uri(item.Cover));
                cb.title.Text ="GTM的数据没title写数据的吃屎了么";
                cb.items = item;
                list.Add(cb);
            }
            this.CateFlipView.ItemsSource = list;
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

        //分区图标添加
        private void AddCategoryButton()
        {
            this.CategoryStakPanel.Children.Clear();
            foreach (var item in _categoryPageData.dicOfData.ToList()[1].Value.ACContetns)
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

        private void AddRecommendData()
        {
            if (_categoryPageData.dicOfData.Count < 6)
            {
                RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Collapsed;
                return;
            }
            RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Visible;
            var items = _categoryPageData.dicOfData.ToList();
            if (items[3].Value.Type!="videos")
            {
                RecommendListView.Visibility = RecommendStackPanel.Visibility = Visibility.Collapsed;
                return;
            }
            RecommendListView.ItemsSource = items[3].Value.ACContetns;
        }
        private void AddRankData()
        {
            var items = _categoryPageData.dicOfData.ToList();
            RankListView.ItemsSource = items[2].Value.ACContetns;
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
    }
}
