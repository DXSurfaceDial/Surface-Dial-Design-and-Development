using HelloDial.Controls;
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
        private DialColorControl dialControl;

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

            myController.ScreenContactStarted += MyController_ScreenContactStarted;
            myController.ScreenContactContinued += MyController_ScreenContactContinued;
            myController.ScreenContactEnded += MyController_ScreenContactEnded;

            myController.RotationChanged += MyController_RotationChanged;
        }

        private async void MyController_ScreenContactStarted(RadialController sender, RadialControllerScreenContactStartedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (null == dialControl)
                {
                    dialControl = new DialColorControl();
                    DialCanvas.Children.Add(dialControl);
                }
                
                dialControl.Width = 300;
                dialControl.Height = 300;

                Canvas.SetLeft(dialControl, args.Contact.Position.X - 150);
                Canvas.SetTop(dialControl, args.Contact.Position.Y - 150);

                dialControl.Visibility = Visibility.Visible;
            });
        }

        private async void MyController_ScreenContactContinued(RadialController sender, RadialControllerScreenContactContinuedEventArgs args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Canvas.SetLeft(dialControl, args.Contact.Position.X - 150);
                    Canvas.SetTop(dialControl, args.Contact.Position.Y - 150);
                });
            }
        }

        private async void MyController_ScreenContactEnded(RadialController sender, object args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialControl.Visibility = Visibility.Collapsed;
                });
            }
        }

        private async void MyController_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            if (null != dialControl)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    dialControl.Rotation += args.RotationDeltaInDegrees;
                    if (dialControl.Rotation > 0)
                    {
                        dialControl.Rotation = 0;
                    }
                    else if (dialControl.Rotation < -315)
                    {
                        dialControl.Rotation = -315;
                    }
                    MainGrid.Background = dialControl.ColorBrush;
                });
            }
        }

        private void ColorMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            Debug.WriteLine("Item invoked");
            myController.Menu.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            myController.Menu.Items.Remove(screenColorMenuItem);
            Frame.GoBack();
        }
    }
}
