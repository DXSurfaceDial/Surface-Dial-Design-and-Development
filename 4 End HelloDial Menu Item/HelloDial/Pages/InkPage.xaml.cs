using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class InkPage : Page
    {
        private InkCanvas inkCanvas = null;
        private InkToolbar inkToolbar = null;

        public InkPage()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AddInkCanvas();
            AddInkToolbar();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MainGrid.Children.Remove(inkToolbar);
            MainGrid.Children.Remove(inkCanvas);
            //release the reference 
            inkToolbar = null;
            inkCanvas = null;
        }

        private void AddInkCanvas()
        {
            inkCanvas = new InkCanvas();
            inkCanvas.IsHitTestVisible = false;
            MainGrid.Children.Add(inkCanvas);
        }

        private void AddInkToolbar()
        {
            inkToolbar = new InkToolbar();
            inkToolbar.TargetInkCanvas = inkCanvas;
            inkToolbar.HorizontalAlignment = HorizontalAlignment.Center;
            inkToolbar.VerticalAlignment = VerticalAlignment.Bottom;
            inkToolbar.Margin = new Thickness(0, 0, 0, 105);
            MainGrid.Children.Add(inkToolbar);
        }
    }
}
