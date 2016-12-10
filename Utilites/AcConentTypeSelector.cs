using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AcFunVideo.Model;

namespace AcFunVideo.Utilites
{
    public class AcConentTypeSelector:DataTemplateSelector
    {
        public DataTemplate VideoTemplate { get; set; }
        public DataTemplate BangumiTemplate { get; set; }
        public DataTemplate ArticleTemplate { get; set; }
        public DataTemplate SpecialTemplate { get; set; }
        public DataTemplate UserTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var content = (AcContent) item;
            switch (content.AcContentType)
            {
                case AcContentType.Articles:
                    return ArticleTemplate;
                case AcContentType.Bangumis:
                    return BangumiTemplate;
                case AcContentType.Special:
                    return SpecialTemplate;
                case AcContentType.User:
                    return UserTemplate;
                case AcContentType.Videos:
                    return VideoTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
