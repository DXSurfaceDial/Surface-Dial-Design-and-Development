using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HelloDial.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnScreenVisualPage : Page
    {
        private RadialController myController;
        private RadialControllerMenuItem screenColorMenuItem;

        public OnScreenVisualPage()
        {
            this.InitializeComponent();

            RadialControllerConfiguration myConfiguration = RadialControllerConfiguration.GetForCurrentView();
            myConfiguration.SetDefaultMenuItems(new[]
                {
                    RadialControllerSystemMenuItemKind.Volume,
                    RadialControllerSystemMenuItemKind.Scroll
                });

            // Create a reference to the RadialController.
            myController = RadialController.CreateForCurrentView();

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
              RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/dial_icon_custom_visual.png"));

            // Create a menu item for the custom tool.
            screenColorMenuItem =
              RadialControllerMenuItem.CreateFromIcon("Screen Color", icon);

            // Add the custom tool to the RadialController menu.
            myController.Menu.Items.Add(screenColorMenuItem);
            
            screenColorMenuItem.Invoked += ColorMenuItem_Invoked;
        }

        private void ColorMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            Debug.WriteLine("Item invoked");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            myController.Menu.Items.Remove(screenColorMenuItem);
            Frame.GoBack();
        }
    }
}
