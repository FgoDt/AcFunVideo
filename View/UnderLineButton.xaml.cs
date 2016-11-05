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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AcFunVideo.View
{
    public sealed partial class UnderLineButton : UserControl
    {
        public Grid button;
        public TextBlock title;
        public Border underline;
        public UnderLineButton()
        {
            this.InitializeComponent();
            this.button = defaultButton;
            this.title = Title;
            this.underline = UnderLine;
            this.Loaded += UnderLineButton_Loaded;
        }

        private void UnderLineButton_Loaded(object sender, RoutedEventArgs e)
        {
            this.underline.Width = this.title.ActualWidth + 4;
        }
    }
}
