using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AcFunVideo
{
   public partial class App:Application
    {
        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            base.OnFileActivated(args);
            //todo
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args.Kind==ActivationKind.Protocol)
            {
                Frame rootFrame = CreateRootFrame();
                if (rootFrame.Content==null)
                {
                //todo
                }
            }
        }
    } 
}
