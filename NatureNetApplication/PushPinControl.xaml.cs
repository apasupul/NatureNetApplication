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
        /// <summary>
        /// collection to store the list of images queried from the database for a perticular pinlocation 
        /// currently only loads pushpin 
        /// 

        ///
        /// TODO : finish querying and loading images
        ///
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

        private Microsoft.Maps.MapControl.WPF.Location pinLocation;
        int localdatabaseindex;
        public PushPinControl()
        {
            InitializeComponent();
            Push_Image_container.ItemsSource = photos;

        }
        /// <summary>
        /// Initiazlize the pushpin control with only geo-cordinate location data
        /// currrently not used
        /// </summary>
        /// <param name="pinLocation"></param>
        public PushPinControl(Microsoft.Maps.MapControl.WPF.Location pinLocation)
        {
            InitializeComponent();

            this.pinLocation = pinLocation;
            Push_Image_container.DataContext = "libraryContainer1";
            _Latitude = pinLocation.Latitude;
            _longitude = pinLocation.Longitude;
            Push_Image_container.ItemsSource = Names;
        }
        /// <summary>
        /// Initiazlize the pushpin control with geo-cordinate location data and index positon data from the database
        /// currrently not used
        /// </summary>
        /// <param name="pinLocation"></param>
        public PushPinControl(Microsoft.Maps.MapControl.WPF.Location pinLocation, int databaseindex)
        {
            InitializeComponent();

            this.pinLocation = pinLocation;
            Push_Image_container.DataContext = "libraryContainer1";
            _Latitude = pinLocation.Latitude;
            _longitude = pinLocation.Longitude;
            Push_Image_container.ItemsSource = Names;
            localdatabaseindex = databaseindex;

        }
        /// <summary>
        /// onloaded event for the pushpin control 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            ///
            /// TODO : load images associated this pushpin's geo location
            ///
        }
        /// <summary>
        /// Detects when soemthing is droped on the pushpin control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCursorDrop(object sender, SurfaceDragDropEventArgs args)
        {
            ///
            /// TODO : not currently used
            ///
        }
        /// <summary>
        /// deleats the push pin when the used clicks on the delete pushhpin option from the menu on the pushpin control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Pushpincontrol.Visibility = Visibility.Collapsed;

        }
        /// <summary>
        /// make the image colelction container visible when the user clicks on add photos 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementMenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Push_Image_container.Visibility = Visibility.Visible;

        }
        /// <summary>
        /// when the user clicks on the doen adding menu option form the menu on the pushpin user control it checks if user has any images associated with the image collection 
        /// if there are any images then it stores the associated information and stores it in the databse , if there are no images the pushpin is removed and not stored in teh database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementMenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (Names.Count == 0)
            {
                Pushpincontrol.Visibility = Visibility.Collapsed;
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
                int asd = (Convert.ToInt32(value4));
                cmd.CommandText = "INSERT INTO PushPin_location (x_position, y_position, pin_ID) VALUES ('" + _Latitude + "', '" + _longitude + "', '" + (++asd) + "')";
                cmd.ExecuteNonQuery();
                foreach (String s in Names)
                {
                    string result;

                    result = System.IO.Path.GetFileName(s);
                    cmd.CommandText = "INSERT INTO Pushpins_to_images (pin_ID, image_name) VALUES ('" + asd + "', '" + result + "')";
                    cmd.ExecuteScalar();
                }
                Push_Image_container.Visibility = Visibility.Hidden;
            }
        }

        
        private void Push_Image_container_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            return;
            //object neededdata = e.Cursor.Data;
            // TODO : ( abhijit ) : process dropped collections to add photos into the pushpin collection
            //Content test = e.Cursor.Data as Content;
            ////test.p = 
            //FrameworkElement findSource = e.OriginalSource as FrameworkElement;

            //SurfaceDragCursor droppingCursor = e.Cursor;
            //if (!Names.Contains(test.p.ToString()))
            //{
            //    if (test.p.ToString() == null)
            //    { }
            //    else
            //    {
            //        Names.Add(test.p.ToString());
            //    }
            //}
            //Push_Image_container.DataContext = this;
            //Push_Image_container.ItemsSource = names;
            //e.Handled = true;

        }

        private void Imageholder_Drop(object sender, DragEventArgs e)
        {
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;


            if (!photos.Contains(e.Data.ToString()))
            {
                photos.Add(e.Data.ToString());

            }

            Push_Image_container.ItemsSource = photos;
        }

        private void StackView_Drop(object sender, DragEventArgs e)
        {

        }

       
        /// <summary>
        /// sets and item dragged from a collection to enabled ( from greyed out to not greyout )
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void test_DragEnter(object sender, SurfaceDragDropEventArgs e)
        {
            SurfaceDragCursor droppingCursor = e.Cursor;
            LibraryBar currentbar = e.Cursor.DragSource as LibraryBar;
            if (currentbar != null)
            {
                currentbar.SetIsItemDataEnabled(e.Cursor.Data, true);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void test_Drop(object sender, SurfaceDragDropEventArgs e)
        //{
        //    SurfaceDragCursor droppingCursor = e.Cursor;
        //    LibraryBar currentbar = e.Cursor.DragSource as LibraryBar;
        //    if (currentbar != null)
        //    {
        //        currentbar.SetIsItemDataEnabled(e.Cursor.Data, true);
        //    }

        //}
    }
}
