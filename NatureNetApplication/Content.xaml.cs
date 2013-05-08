using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Surface.Presentation.Controls;

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        public object p;
        List<string> photos = new List<string>();
        public Content()
        {
            InitializeComponent();
        }

        public Content(object p)
        {

            // TODO: Complete member initialization
            this.p = p;
            InitializeComponent();
            DataContext = this;
            imagebox.BeginInit();
            BitmapImage bmp = new BitmapImage(new Uri(p.ToString(), UriKind.Absolute));
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            imagebox.Source = bmp;
            imagebox.EndInit();
            FileInfo info = new FileInfo(p.ToString());
            String name = info.Name;
            var directory = System.IO.Path.GetDirectoryName(p.ToString());



            //BitmapImage img = new BitmapImage();
            //var uriSource = new Uri((@"I:\NatureNetApplication\NatureNetApplication\bin\Debug\Resources\Abhijit@gmail.com\beach_29-wallpaper-1600x900.jpg"));
            //img.BeginInit();
            //img.UriSource = uriSource;
            //img.EndInit();
            //var pixel = img.PixelHeight;
            //var pixelwid = img.PixelWidth;
            //Image testimage = new Image();
            //testimage.Source = img.UriSource; 
            //var uri =new System.Uri(@"I:\NatureNetApplication\NatureNetApplication\bin\Debug\Resources\Abhijit@gmail.com\beach_29-wallpaper-1600x900.jpg");
            //var converted = uri.AbsoluteUri;
            //imagebox.SetValue(Image.SourceProperty, converted);
            //imagebox.Source = myImage;
            

        }
        public string SourceUri
        {
            get
            {
                return p.ToString();
            }
        }

        private void SurfaceButton_TouchDown(object sender, TouchEventArgs e)
        {
            //DependencyObject parent = VisualTreeHelper.GetParent(this);
            //ScatterViewItem svi = null;
            //while (parent as DragDropScatterView == null)
            //{
            //    if (parent is ScatterViewItem)
            //        svi = parent as ScatterViewItem;
            //    parent = VisualTreeHelper.GetParent(parent);
            //}

            //foreach (object item in ((DragDropScatterView)parent).Items)
            //{
            //    if (item is ScatterViewItem)
            //        if (((ScatterViewItem)item).Content is LibraryContainer)
            //        {
            //            ((LibraryContainer)((ScatterViewItem)item).Content).SetIsItemDataEnabled(this.DataContext, true);
            //            break;
            //        }
            //}

            ////LibraryItems1.SetIsItemDataEnabled(o, true);
            //((DragDropScatterView)parent).Items.Remove(this.DataContext);
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            ScatterViewItem svi = null;
            while (parent as ScatterView == null)
            {
                if (parent is ScatterViewItem)
                    svi = parent as ScatterViewItem;
                parent = VisualTreeHelper.GetParent(parent);
            }

            ((ScatterView)parent).Items.Remove(svi);

        }

        private void SurfaceButton_Click(object sender, RoutedEventArgs e)
        {
            //DependencyObject parent = VisualTreeHelper.GetParent(this);
            //ScatterViewItem svi = null;
            //while (parent as DragDropScatterView == null)
            //{
            //    if (parent is ScatterViewItem)
            //        svi = parent as ScatterViewItem;
            //    parent = VisualTreeHelper.GetParent(parent);
            //}

            //foreach (object item in ((DragDropScatterView)parent).Items)
            //{
            //    if (item is ScatterViewItem)
            //        if (((ScatterViewItem)item).Content is LibraryContainer)
            //        {
            //            ((LibraryContainer)((ScatterViewItem)item).Content).SetIsItemDataEnabled(this.DataContext, true);
            //            break;
            //        }
            //}

            ////LibraryItems1.SetIsItemDataEnabled(o, true);
            //((DragDropScatterView)parent).Items.Remove(this.DataContext);
            DependencyObject parent = VisualTreeHelper.GetParent(this);
            ScatterViewItem svi = null;
            while (parent as ScatterView == null)
            {
                if (parent is ScatterViewItem)
                    svi = parent as ScatterViewItem;
                parent = VisualTreeHelper.GetParent(parent);
            }

            ((ScatterView)parent).Items.Remove(svi);
        }

        private void SurfaceButton_Click_1(object sender, RoutedEventArgs e)
        {
            //DependencyObject parent = VisualTreeHelper.GetParent(this);
            //ScatterViewItem svi = null;
            //while (parent as ScatterView == null)
            //{
            //    if (parent is ScatterViewItem)
            //        svi = parent as ScatterViewItem;
            //    parent = VisualTreeHelper.GetParent(parent);
            //}
            //ScatterViewItem svi = null;
            //FrameworkElement findsource = e.Source as FrameworkElement;
            //while (svi == null && findsource != null)
            //{
            //    if ((svi = findsource as ScatterViewItem) == null)
            //    {
            //        findsource = VisualTreeHelper.GetParent(findsource) as FrameworkElement;
            //    }
            //}
            //((ScatterView)parent).Items.Remove(svi);
            //ScatterViewItem svi = null;
            //FrameworkElement findsource = e.Source as FrameworkElement;
            //while (svi == null && findsource != null)
            //{
            //    if ((svi = findsource as ScatterViewItem) == null)
            //    {
            //        findsource = VisualTreeHelper.GetParent(findsource) as FrameworkElement;
            //    }
            //}
            //ScatterView.Items.Remove(svi);//or which you prefer to use Collapsed

            DependencyObject parent = VisualTreeHelper.GetParent(this);
            ScatterViewItem svi = null;
            while (parent as DragDropScatterView == null)
            {
                if (parent is ScatterViewItem)
                    svi = parent as ScatterViewItem;
                parent = VisualTreeHelper.GetParent(parent);
            }

            

            //LibraryItems1.SetIsItemDataEnabled(o, true);
            ((DragDropScatterView)parent).Items.Remove(this.DataContext);
        }

        
    }
}
