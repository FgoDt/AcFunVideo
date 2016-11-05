using AcFunVideo.Model;
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
    public sealed partial class NormalView : UserControl
    {
        public FDImgBox img;
        public TextBlock title;
        public TextBlock upName;
        public NormalView()
        {
            this.InitializeComponent();
            img = IMG;
            title = TITLE;
            upName = UPNAME;
        }

        public void AddAllData(AcContent data)
        {
            img.Width = this.Width;
            img.Height = this.Height-60 ;
            title.Width = img.Width;
            //var temp = new BitmapImage();
            img.UriSource = new Uri(data.Cover);

            title.Text = data.Title;
      
            if (data.User == null || data.User.Username == null || data.User.Username == string.Empty)
            {
                this.Height = this.Height - 15;
                sp.Children.Remove(upName);
                return;

            }
            upName.Text ="UP: "+ data.User.Username;
        }
    }
}
