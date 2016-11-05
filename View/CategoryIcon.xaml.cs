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
    public sealed partial class CategoryIcon : UserControl
    {
        public Image IMG { get; set; }
        public Grid RootGrid { get; set; }
        public TextBlock Title { get; set; }
        public CategoryIcon()
        {
            this.InitializeComponent();
            this.IMG = img;
            RootGrid = rootGrid;
            Title = title;
        }

        public void AddData(MainPageCategoryData data)
        {
            
            img.Source = new BitmapImage(new Uri("ms-appx:///"+data.img));
            title.Text = data.title;
            if (data.borderColor!=null)
            {
                rootGrid.BorderThickness = new Thickness(2);
                rootGrid.BorderBrush = data.borderColor;
                title.Foreground = data.borderColor;
            }
            rootGrid.Background = data.backColor;
        }
    }
}
