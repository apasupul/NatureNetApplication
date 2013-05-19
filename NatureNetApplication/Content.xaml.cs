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

            DependencyObject parent = VisualTreeHelper.GetParent(this);
            ScatterViewItem svi = null;
            while (parent as DragDropScatterView == null)
            {
                if (parent is ScatterViewItem)
                    svi = parent as ScatterViewItem;
                parent = VisualTreeHelper.GetParent(parent);
            }

            ((DragDropScatterView)parent).Items.Remove(this.DataContext);
        }


    }
}
