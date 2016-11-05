using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class UnderLineSwitchButton : UserControl
    {

        List<UnderLineButton> listOfButton = new List<UnderLineButton>();
        public Brush TextColor { get; set; }
        public Brush UnderLineColor { get; set; }
        public Brush SelectColor { get; set; }
        public Brush UnSelectColor { get; set; }
        public int SelectIndex { get; set; }
        public UnderLineButton defaultButton;
      //  public event TappedEventHandler Click;
        
        public UnderLineSwitchButton()
        {
            this.InitializeComponent();
            listOfButton.Add(DefualtButton);
            TextColor = DefualtButton.title.Foreground;
            UnderLineColor = DefualtButton.underline.Background;
            this.SelectColor = UnderLineColor;
            DefualtButton.Tapped += Button_Tapped;
            UnSelectColor = new SolidColorBrush(Colors.Black);
            this.Loaded += UnderLineSwitchButton_Loaded;
            this.defaultButton = DefualtButton;
        }

        private void UnderLineSwitchButton_Loaded(object sender, RoutedEventArgs e)
        {
            ReSizeButton();
        }

        public void AddButton(string title)
        {
            var button = new UnderLineButton();
            button.title.Text = title;
            button.title.Foreground = TextColor;
            button.underline.Background = UnderLineColor;
            button.Tapped += Button_Tapped;
            button.title.Opacity = 0.6;
            button.title.Foreground = UnSelectColor;
            button.underline.Background = null;
            listOfButton.Add(button);
            ButtonContent.Children.Add(button);
            ReSizeButton();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            for (int i = 0; i < listOfButton.Count; i++)
            {
                if ((sender as UnderLineButton)==listOfButton[i])
                {
                    this.SelectIndex = i;
                    this.SwitchButton(i);
                    return;
                }
            }
            this.SelectIndex = 0;
        }


        private void ReSizeButton()
        {
            var width = this.ActualWidth / listOfButton.Count;
            foreach (var UnderLineButton in listOfButton)
            {
                UnderLineButton.button.Width = width;
            }
        }

        public void SwitchButton(int i)
        {

            for (int j = 0; j < listOfButton.Count; j++)
            {
                if (j==i)
                {
                    listOfButton[j].title.Foreground = SelectColor;
                    listOfButton[j].underline.Background = SelectColor;
                    listOfButton[j].title.Opacity = 1;
                }
                else
                {
                    listOfButton[j].title.Foreground = UnSelectColor;
                    listOfButton[j].title.Opacity = 0.6;
                    listOfButton[j].underline.Background = null;
                }
            }

         
        }


    }
}
