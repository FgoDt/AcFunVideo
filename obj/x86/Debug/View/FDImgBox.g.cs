﻿#pragma checksum "C:\Users\li\Source\Repos\AcFunVideo\AcFunVideo\View\FDImgBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9F4A2D2EC486BAB9E005B9A6858E0AA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AcFunVideo.View
{
    partial class FDImgBox : 
        global::Windows.UI.Xaml.Controls.UserControl, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_Image_Stretch(global::Windows.UI.Xaml.Controls.Image obj, global::Windows.UI.Xaml.Media.Stretch value)
            {
                obj.Stretch = value;
            }
        };

        private class FDImgBox_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IFDImgBox_Bindings
        {
            private global::AcFunVideo.View.FDImgBox dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.Image obj2;

            public FDImgBox_obj1_Bindings()
            {
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 2:
                        this.obj2 = (global::Windows.UI.Xaml.Controls.Image)target;
                        break;
                    default:
                        break;
                }
            }

            // IFDImgBox_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
            }

            // FDImgBox_obj1_Bindings

            public void SetDataRoot(global::AcFunVideo.View.FDImgBox newDataRoot)
            {
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::AcFunVideo.View.FDImgBox obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Stretch(obj.Stretch, phase);
                    }
                }
            }
            private void Update_Stretch(global::Windows.UI.Xaml.Media.Stretch obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_Image_Stretch(this.obj2, obj);
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    this.GifImage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 16 "..\..\..\View\FDImgBox.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.GifImage).Loaded += this.GifImage_Loaded;
                    #line 18 "..\..\..\View\FDImgBox.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.GifImage).Unloaded += this.GifImage_Unloaded;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.UserControl element1 = (global::Windows.UI.Xaml.Controls.UserControl)target;
                    FDImgBox_obj1_Bindings bindings = new FDImgBox_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            }
            return returnValue;
        }
    }
}

