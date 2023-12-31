using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;

namespace PixeLList.Pages
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Window
    {
        public WelcomePage()
        {
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void extendedSplashImage_Loaded(object sender, RoutedEventArgs e)
        {
            string image = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "p1x3lherz-0PMXCNxp8O1N9q4HrOOGqZ_mini.png");
            extendedSplashImage.Source = new BitmapImage(new Uri(image));

        }
    }
}
