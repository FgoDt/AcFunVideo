using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AcFunVideo.Model;
namespace AcFunVideo.Class
{
   public class ACDataTemplates:DataTemplateSelector
    {
        public DataTemplate CategoryBannerTemplate { get; set; }
        public DataTemplate NormalViewTemplate { get; set; }
        public DataTemplate CategoryIconTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var model = item as AcContent;
            if (model==null)
            {
                return CategoryBannerTemplate;
            }
            var typeName = model.Type;
            if (typeName=="channels")
            {
                return CategoryIconTemplate;
            }
            return NormalViewTemplate;
        }
    }
}
