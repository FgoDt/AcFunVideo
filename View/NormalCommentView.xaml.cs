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
    public sealed partial class NormalCommentView : UserControl
    {
        public StackPanel rootStackPanel;
        public StackPanel commentStackPanel;
        public TextBlock usrTextBox;
        public RichTextBlock commentbox;
        public Image usrImg;
        public Windows.UI.Xaml.Documents.Paragraph commentParagraph;
        public NormalCommentView()
        {
            this.InitializeComponent();
            this.commentStackPanel = CommentStackPanel;
            this.usrTextBox = UsrNameBox;
            this.commentbox = CommentContentBox;
            this.usrImg = UsrImgBox;
            this.commentParagraph = CommentParagraph;
            this.rootStackPanel = RootStackPanel;
        }
    }
}
