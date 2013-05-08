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
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;


namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for PushPinControl.xaml
    /// </summary>
   // 

   // ObservableCollection
    public partial class PushPinControl : UserControl

    {
        private ObservableCollection<String> names;
        public ObservableCollection<String> Names
        {
            get
            {
                if (names == null)
                {
                    names = new ObservableCollection<String>();
                }

                return names;
            }
        }
        Double _Latitude, _longitude;
        List<string> photos = new List<string>();
        //ICollectionView view1 = CollectionViewSource.GetDefaultView(photos);
        private Microsoft.Maps.MapControl.WPF.Location pinLocation;
        int localdatabaseindex;
        public PushPinControl()
        {
            InitializeComponent();
            Push_Image_container.ItemsSource = photos;
            
       }

        public PushPinControl(Microsoft.Maps.MapControl.WPF.Location pinLocation)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.pinLocation = pinLocation;
            Push_Image_container.DataContext = "libraryContainer1";
            _Latitude= pinLocation.Latitude ;
           _longitude= pinLocation.Longitude ;
           Push_Image_container.ItemsSource = Names;
          // Loaded += new RoutedEventHandler(OnLoaded);
        }

        public PushPinControl(Microsoft.Maps.MapControl.WPF.Location pinLocation, int databaseindex)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.pinLocation = pinLocation;
            Push_Image_container.DataContext = "libraryContainer1";
            _Latitude = pinLocation.Latitude;
            _longitude = pinLocation.Longitude;
            Push_Image_container.ItemsSource = Names;
            localdatabaseindex = databaseindex;
         //   Loaded += new RoutedEventHandler(OnLoaded);
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

          //  PushPinControl.   AddDropHandler(this, OnCursorDrop);
           // AddHandler(PushPinControl.DropEvent, new SurfaceDragDropEventArgs(OnCursorDrop));
            //AddHandler(ScatterViewItem.ContainerManipulationStartedEvent, new ContainerManipulationStartedEventHandler(OnManipulationStarted));
        }

        private void OnCursorDrop(object sender, SurfaceDragDropEventArgs args)
        {

        }
        private void ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            test.Visibility = Visibility.Collapsed;

        }

        private void ElementMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Imageholder.Visibility = Visibility.Visible;

        }

        private void ElementMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (Names.Count == 0)
            {
                test.Visibility = Visibility.Collapsed;
            }
            else
            {
                SqlCeConnection conn = null;

                string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                string connectionString = string.Format("Data Source=" + filesPath);
                conn = new SqlCeConnection(connectionString);
                SqlCeCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) AS NumberOfOrders FROM PushPin_location";
                conn.Open();
                object value4 = cmd.ExecuteScalar();
                int asd = (Convert.ToInt32( value4));
                cmd.CommandText = "INSERT INTO PushPin_location (x_position, y_position, pin_tag) VALUES ('"+_Latitude+"', '"+_longitude+"', '"+(++asd)+"')";
                cmd.ExecuteNonQuery();
                foreach (String s in Names)
                {
                    string result;

                    result = System.IO.Path.GetFileName(s);
                    cmd.CommandText = "INSERT INTO Pushpins_to_images (push_number, image_name) VALUES ('"+asd+"', '"+result+"')";
                    cmd.ExecuteScalar();
                }
                Imageholder.Visibility = Visibility.Hidden;
            }
        }

        private void Scatter_PreviewTouchDown(object sender, TouchEventArgs e)
        {
        }

        private void Push_Image_container_PreviewDragEnter(object sender, SurfaceDragDropEventArgs e)
        {


        }

        private void Push_Image_container_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            object neededdata = e.Cursor.Data;
           // Image test = new Image();
           // BitmapImage data = e.Cursor.Data as BitmapImage;
           // //int asas = data.PixelWidth;
           //// int adasa = data.PixelHeight;
           // List<string> photos = new List<string>();
           // //BitmapImage bmp = new BitmapImage(new Uri(e.Cursor.Data.ToString(), UriKind.Absolute));
           // //bmp.CacheOption = BitmapCacheOption.OnLoad;
           // photos.Add(e.Cursor.Data.ToString());
           // Push_Image_container.ItemsSource = photos;
           // e.Handled = true;
            
            //e.
            Content test = e.Cursor.Data as Content;
            //test.p = 
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
           // SurfaceListBoxItem draggedElement = null;
            SurfaceDragCursor droppingCursor = e.Cursor;
           // var svi = ItemContainerGenerator.ContainerFromItem(neededdata) as Content;
          // string test= neededdata.p;
          //      var svi = ItemContainerGenerator.ContainerFromItem(droppingCursor.Data) as ScatterViewItem;
           
            if (!Names.Contains(test.p.ToString()))
             {
                 if (test.p.ToString() == null)
                 { }
                 else
                 {
                     Names.Add(test.p.ToString());
                 }
             }
             Push_Image_container.DataContext = this;
             Push_Image_container.ItemsSource = names;
             e.Handled = true;
            // Find the SurfaceListBoxItem object that is being touched.
           // while (draggedElement == null && findSource != null)
           // {
           //     if ((draggedElement = findSource as SurfaceListBoxItem) == null)
           //     {
           //         findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
           //     }
           // }

           // if (draggedElement == null)
           // {
           //     return;
           // }

           //// PhotoData data = draggedElement.Content as PhotoData;

           // // Create the cursor visual
           // ContentControl cursorVisual = new ContentControl()
           // {
           //     Content = draggedElement.DataContext,
           //     Style = FindResource("CursorStyle") as Style
           // };

           // // Create a list of input devices. Add the touches that
           // // are currently captured within the dragged element and
           // // the current touch (if it isn't already in the list).
           // List<InputDevice> devices = new List<InputDevice>();
           // devices.Add(e.TouchDevice);
           // foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin)
           // {
           //     if (touch != e.TouchDevice)
           //     {
           //         devices.Add(touch);
           //     }
           // }

           // // Get the drag source object
           // ItemsControl dragSource = ItemsControl.ItemsControlFromItemContainer(draggedElement);

           // SurfaceDragDrop.BeginDragDrop(
           //     dragSource,
           //     draggedElement,
           //     cursorVisual,
           //     draggedElement.DataContext,
           //     devices,
           //     DragDropEffects.Move);

           // // Prevents the default touch behavior from happening and disrupting our code.
           // e.Handled = true;

           // // Gray out the SurfaceListBoxItem for now. We will remove it if the DragDrop is successful.
           // draggedElement.Opacity = 0.5;

        }

        private void Imageholder_Drop(object sender, DragEventArgs e)
        {
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
            // SurfaceListBoxItem draggedElement = null;
           
            if (!photos.Contains(e.Data.ToString()))
            {
                photos.Add(e.Data.ToString());

            }

            Push_Image_container.ItemsSource = photos;
        }

        private void StackView_Drop(object sender, DragEventArgs e)
        {
        
        }
    }
}
