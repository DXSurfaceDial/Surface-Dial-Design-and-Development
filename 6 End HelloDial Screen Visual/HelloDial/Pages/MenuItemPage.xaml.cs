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
    public sealed partial class MenuItemPage : Page
    {
        private RadialController myController;
        private RadialControllerMenuItem selectImageMenuItem;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(MenuItemPage), new PropertyMetadata(0));

        public MenuItemPage()
        {
            this.InitializeComponent();

            RadialControllerConfiguration myConfiguration = RadialControllerConfiguration.GetForCurrentView();
            //determine which operating system menu items should be shown 
            myConfiguration.SetDefaultMenuItems(new[]
                {
                    RadialControllerSystemMenuItemKind.Volume,
                    RadialControllerSystemMenuItemKind.NextPreviousTrack,
                    RadialControllerSystemMenuItemKind.Scroll
                });

            // Create a reference to the RadialController.
            myController = RadialController.CreateForCurrentView();

            // Create an icon for the custom tool.
            RandomAccessStreamReference icon =
              RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/dial_icon_custom_item.png"));

            // Create a menu item for the custom tool.
            selectImageMenuItem =
              RadialControllerMenuItem.CreateFromIcon("Select Image", icon);

            // Add the custom tool to the RadialController menu.
            myController.Menu.Items.Add(selectImageMenuItem);

            // Declare input handlers for the RadialController.
            myController.RotationChanged += MyController_RotationChanged;

            selectImageMenuItem.Invoked += SelectImageItem_Invoked;
        }

        private async void MyController_RotationChanged(RadialController sender, 
            RadialControllerRotationChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    if (args.RotationDeltaInDegrees > 0
                    && SelectedIndex < 6)
                    {
                        SelectedIndex++;
                    }
                    else if (args.RotationDeltaInDegrees < 0
                        && SelectedIndex > 0)
                    {
                        SelectedIndex--;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        private void SelectImageItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            Debug.WriteLine("Button Invoked");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            myController.Menu.Items.Remove(selectImageMenuItem);
            Frame.GoBack();
        }
    }
}
