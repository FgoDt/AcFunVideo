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

namespace AcFunVideo.View
{
    public sealed partial class CategoryBanner : UserControl
    {
        public FlipView flipview;
        public Grid rootGrid;
        public AcContent items;
        public Image cover;
        public TextBlock title;
        public CategoryBanner()
        {
            this.InitializeComponent();
            this.rootGrid = RootGrid;
            this.cover = Cover;
            this.title = Title;
        }
    }
}
