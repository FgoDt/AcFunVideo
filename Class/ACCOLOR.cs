using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace AcFunVideo.Class
{
    /// <summary>
    /// 所有自定义颜色
    /// </summary>
    public class ACCOLOR
    {
        public static SolidColorBrush ACDEEPBLU = new SolidColorBrush(Color.FromArgb(255, 0, 120, 215));
        public static SolidColorBrush ACBLU = new SolidColorBrush(Color.FromArgb(255, 39, 200, 215));
        public static SolidColorBrush ACYELLOW = new SolidColorBrush(Color.FromArgb(255, 255, 180, 0));
        public static SolidColorBrush ACORANGE = new SolidColorBrush(Color.FromArgb(255, 251, 122, 52));
        public static SolidColorBrush ACGREEN = new SolidColorBrush(Color.FromArgb(255, 55, 216, 113));
        public static SolidColorBrush ACRED = new SolidColorBrush(Color.FromArgb(255, 229, 19, 66));
        public static SolidColorBrush colorWith(Color color)
        {
            return new SolidColorBrush(color);
        }
    }


}
