using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HelloDial.Controls
{
    public sealed partial class DialColorControl : UserControl
    {
        private bool rightHanded;

        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); UpdateColorBrush(); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(DialColorControl), new PropertyMetadata(0.0));



        public SolidColorBrush ColorBrush
        {
            get { return (SolidColorBrush)GetValue(ColorBrushProperty); }
            set { SetValue(ColorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBrushProperty =
            DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(DialColorControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        private async void UpdateColorBrush()
        {
            int x = 0, y = 0;

            double angle = (360-Rotation) * Math.PI/180;
            double radius = 295;
            //get the co ords of the pixel based on rotation
            //x = radius cos t;
            //y = radius sin t;
            x = 350 + (int)(radius * Math.Cos(angle));
            y = 353 + (int)(radius * Math.Sin(angle));

            
            Color color = await GetSelectedColor(x, y);

            Debug.WriteLine($"X: {x}, Y: {y} rotation: {Rotation} Color: {color.R}:{color.G}:{color.B}");


            ColorBrush.Color = color;
        }
        byte[] bmpBytes = null;
        BitmapDecoder dec;

        private async Task<Color> GetSelectedColor(int x, int y)
        {
            Color color = new Color();
            if (null == bmpBytes)
            {
                string imagelocation = @"Assets\custom_visual_colour_wheel.png";
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile file = await InstallationFolder.GetFileAsync(imagelocation);
                Stream imagestream = await file.OpenStreamForReadAsync();

                dec = await BitmapDecoder.CreateAsync(imagestream.AsRandomAccessStream());

                var data = await dec.GetPixelDataAsync();

                bmpBytes = data.DetachPixelData();
            }
            color = GetPixel(bmpBytes, x, y, dec.PixelWidth, dec.PixelHeight);
            
            return color;
        }

        public Color GetPixel(byte[] pixels, int x, int y, uint width, uint height)
        {
            int i = y;
            int j = x;
            int k = (i * (int)width + j) * 4;
            var b = pixels[k + 0];
            var g = pixels[k + 1];
            var r = pixels[k + 2];
            return Color.FromArgb(255, r, g, b);
        }

        public DialColorControl()
        {
            this.InitializeComponent();
            //handedness for on-screen UI
            Windows.UI.ViewManagement.UISettings settings = new Windows.UI.ViewManagement.UISettings();
            rightHanded = (settings.HandPreference == Windows.UI.ViewManagement.HandPreference.RightHanded);
            //if left handed then rotate the entire control 180
            if (!rightHanded)
            {
                RotateTransform trans = new RotateTransform();
                trans.Angle = 180;
                colorControl.RenderTransform = trans;
            }
        }
    }
}
