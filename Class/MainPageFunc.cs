using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AcFunVideo.Model;
using AcFunVideo.View;
using Windows.UI.Xaml.Media.Imaging;

namespace AcFunVideo.Class
{
   public class MainPageFunc
    {
        public static void  BannerFunc(  FlipView flipView, List<AcContent> data)
        {
            List<BannerView> BannerViewlist = new List<BannerView>();
            foreach (var item in data)
            {
                BannerView bv = new BannerView();
                bv.img.Source = new BitmapImage(new Uri(item.Cover));
                bv.title.Text = item.Title;
                bv.Tapped += Bv_Tapped;
                bv.items = item;
                BannerViewlist.Add(bv);
            }
            flipView.ItemsSource = BannerViewlist;
        }

        private static void Bv_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var bv = sender as BannerView;
            if (bv==null)
            {
                return;
            }
            MainPage.kCurrent.Frame.Navigate(typeof(DetailsPage),bv.items);
        }

        public async static void CategoryFunc(GridView gridView)
        {
            MainPageCategoryDatas mpcd = new MainPageCategoryDatas();
            await mpcd.GetData();
            gridView.ItemsSource = mpcd.list;
        }
    }
}
