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
    public sealed partial class DownlaodView : UserControl
    {
        public Image img;
        public TextBlock titleTB;
        public Button PlayB;
        public TextBlock infoTB;
        public Button deleteB;
        public object Item;
        public Grid rootGrid;
        public DownlaodView()
        {
            this.InitializeComponent();
            img = IMG;
            titleTB = TitleTB;
            infoTB = InfoTB;
            deleteB = Delete;
            rootGrid = RootGrid;
            PlayB = Play;
        }

    }
}
