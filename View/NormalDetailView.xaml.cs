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
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AcFunVideo.View
{
    public sealed partial class NormalDetailView : UserControl
    {
        public Image coverBox;
        public Grid rootGrid;
        public TextBlock titleBox;
        public TextBlock upBox;
        public TextBlock viewsBox;
        public TextBlock viewIcon;
        public NormalDetailView()
        {
            this.InitializeComponent();
            coverBox = CoverBox;
            rootGrid = RootGrid;
            titleBox = TitleBlock;
            upBox = UPNameBox;
            viewsBox = ViewsBox;
            viewIcon = ViewIcon;
        }

        private void ResizeView()
        {
           TextGrid.Width= titleBox.Width = this.rootGrid.Width - this.coverBox.Width - 20;
            UPNameBox.Width = titleBox.Width - ViewIcon.Width - 60;
        }

        public void AddData(AcContent content   )
        {
            ResizeView();
            if (content.Visit==null)
            {
                this.viewsBox.Visibility = this.viewIcon.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (content.Visit.Views!=null)
                {
                    this.viewsBox.Text = content.Visit.Views;
                }
            }
            if (content.User!=null)
            {
                UPNameBox.Text = content.User.Username;
            }
            else
            {
                UPNameBox.Text = "UP：你猜是谁";
            }
            coverBox.Source = new BitmapImage( new Uri(content.Cover));
            titleBox.Text = content.Title;

        }
    }
}
