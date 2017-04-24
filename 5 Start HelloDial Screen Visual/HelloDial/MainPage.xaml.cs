using HelloDial.Pages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloDial
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private void InkBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(InkPage));
        }

        private void ItemBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuItemPage));
        }

        private void VisualBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(OnScreenVisualPage));
        }
    }
}
