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
    /// used to process a image dropped on the scatter view 
    /// </summary>
    public partial class Content : UserControl
    {
        public object p;
        List<string> photos = new List<string>();
        public Content()
        {
            InitializeComponent();
        }
        /// <summary>
        /// makes a container for a dropped image and loads it onto it
        /// </summary>
        /// <param name="p"></param>
        public Content(object p)
        {

            // TODO: Complete member initialization
            //
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
        /// <summary>
        /// stores teh sourceuri /// in this case the link to the location of the image on the physical disk
        /// </summary>
        public string SourceUri
        {
            get
            {
                return p.ToString();
            }
        }
        /// <summary>
        /// detects a touch on close button and close's the image view window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        private void Close_window(object sender, RoutedEventArgs e)
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
