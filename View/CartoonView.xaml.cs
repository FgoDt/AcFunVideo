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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AcFunVideo.View
{
    public sealed partial class CartoonView : UserControl
    {
        public Image cover;
        public TextBlock title;
        public TextBlock update;
        public CartoonView()
        {
            this.InitializeComponent();
            this.cover = Cover;
            this.title = TITLE;
            this.update = UPDATE;
        }

        public void AddData(AcFunVideo.Model.AcContent data)
        {
            this.cover.Height = this.Width * 4 / 3;
            var btImg = new BitmapImage(new Uri(data.Cover));
            this.cover.Source = btImg;
            this.title.Text = data.Title;
            this.update.Text = data.Bangumi.Title;
        }
        public void AddTVData(AcFunVideo.Model.AcContent data)
        {
            this.cover.Height = this.Width * 2 / 3;
            var btImg = new BitmapImage(new Uri(data.Cover));
            this.cover.Source = btImg;
            this.title.Text = data.Title;
            this.update.Text = data.Bangumi.Title;
        }
    }
}
