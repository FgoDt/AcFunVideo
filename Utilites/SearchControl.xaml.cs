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
using AcFunVideo.Model;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AcFunVideo.Utilites
{
    public sealed partial class SearchControl : UserControl
    {
        private Action<SearchUrlData> CallSearchUrlData;
        Popup popup=new Popup();
        private bool SearchTypeLoaded = false;
        private bool SortTypeLoaded = false;
        private bool ChannelLoaded = false;
        public SearchControl(double height, Action<SearchUrlData> action)
        {
            this.InitializeComponent();
            CallSearchUrlData = action;
            popup.Child = this;
            popup.VerticalOffset = height;
            popup.IsLightDismissEnabled = true;
            Width = Window.Current.Bounds.Width;
            popup.Width= Window.Current.Bounds.Width;
            SearchTypeListBox.SelectedIndex = 0;
            SortListBox.SelectedIndex = 0;
            ListBoxChannel.SelectedIndex = 0;
        }

        private void SearchControl_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        private void SearchChannelShow(bool show)
        {
            if (show)
            {
                TextBlockChannel.Visibility=Visibility.Visible;
                ListBoxChannel.Visibility=Visibility.Visible; 
            }
            else
            {
                TextBlockChannel.Visibility = Visibility.Collapsed;
                ListBoxChannel.Visibility = Visibility.Collapsed;
            }
        }

        public void Show()
        {
            Width = Window.Current.Bounds.Width;
            popup.Width = Window.Current.Bounds.Width;
            popup.IsOpen = true;
        }

        public void Close()
        {
            popup.IsOpen = false;
        }
        private void TypeSelection_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            SearchTypeLoaded = true;
            CallAction();
        }

        private void SortType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortTypeLoaded = true;
            CallAction();
        }

        private void ChannelType_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChannelLoaded = true;
            CallAction();
        }
        
        private void CallAction()
        {
            if (SearchTypeLoaded && SortTypeLoaded && ChannelLoaded)
            { 
                var stype=(AcContentType)int.Parse(((ListBoxItem)SearchTypeListBox.SelectedItem).Tag.ToString());
                var sortType = (SortType) int.Parse(((ListBoxItem) SortListBox.SelectedItem).Tag.ToString());
                string channelId = null;
                string url = null;
                if (stype == AcContentType.Videos)
                {
                    ChangedVisibility(Visibility.Visible);
                    channelId = ((ListBoxItem) ListBoxChannel.SelectedItem).Tag.ToString();
                    url ="http://search.app.acfun.cn/search?" +
                         $"sortField={sortType.ToString().ToLower()}&type=2&aiCount=0&spCount=0&fqChannelId={channelId}";
                }
                else if(stype== AcContentType.Bangumis)
                {
                    ChangedVisibility(Visibility.Collapsed);
                    var sort = 0;
                    switch (sortType)
                    {
                        case SortType.Score:
                            sort = 0;
                            break;
                        case SortType.Views:
                            sort = 11;
                            break;
                        case SortType.Comments:
                            sort = 13;
                            break;
                        case SortType.Stows:
                            sort = 15;
                            break;
                        case SortType.ReleaseDate:
                            sort = 1;
                            break;
                    }
                    url ="http://fanju.app.acfun.cn/search/bangumi?sort="+sort;
                }
                else if(stype== AcContentType.Articles)
                { 
                    ChangedVisibility(Visibility.Collapsed);
                    url ="http://search.app.acfun.cn/search?" +
                         $"sortField={sortType.ToString().ToLower()}&type=2&aiCount=0&spCount=0&userCount=0&channelIds=63";
                }
                else if(stype== AcContentType.Special)
                {
                    ChangedVisibility(Visibility.Collapsed);
                    url ="http://search.app.acfun.cn/search?" +
                         $"sortField={sortType.ToString().ToLower()}&type=1&userCount=0";
                }
                else if (stype == AcContentType.User)
                {
                    ChangedVisibility(Visibility.Collapsed);
                    url = "http://search.app.acfun.cn/search?sortField=score&type=2&pageNo=1&pageSize=0&userCount=10&spCount=0";
                }
                CallSearchUrlData?.Invoke(new SearchUrlData {ContentType = stype,Url = url});
            }
        }

        private void ChangedVisibility(Visibility v)
        {
            TextBlockChannel.Visibility = v;
            ListBoxChannel.Visibility = v;
        }
    }
}
