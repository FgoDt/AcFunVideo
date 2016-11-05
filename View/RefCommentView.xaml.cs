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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AcFunVideo.View
{
    public sealed partial class RefCommentView : UserControl
    {
        public RichTextBlock commentContentBox;
        public StackPanel rootStackPanel;
        public Border underLine;
        public TextBlock usrNameBox;
        public Windows.UI.Xaml.Documents.Paragraph commentParagraph;
        public Border rootBorder;
        public Border refbutton;
        public commentContent items { get; set; }
        public RefCommentView()
        {
            this.InitializeComponent();
            this.commentContentBox = this.ContentBox;
            this.rootBorder = RootBorder;
            this.usrNameBox = UsrNameBox;
            this.underLine = UnderLine;
            this.commentParagraph = CommentParagraph;
            this.rootStackPanel = RootStackPanel;
            this.refbutton = RefButton;
        }

        private void RefButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var border = sender as Border;
            border.Visibility = Visibility.Collapsed;
            RootStackPanel.Visibility = Visibility.Visible;
        }
    }
}
