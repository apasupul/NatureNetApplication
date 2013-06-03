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
using System.Data.SqlServerCe;

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for Content.xaml
    /// used to process a image dropped on the scatter view 
    /// </summary>
    public partial class Content : UserControl
    {
        public object p;
        String name;
        List<string> photos = new List<string>();
        DateTime bar;
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
             bar = DateTime.Now;
            // TODO: Complete member initialization
            //
            this.p = p;
            InitializeComponent();
            DataContext = this;
            _imagebox.BeginInit();
            BitmapImage bmp = new BitmapImage(new Uri(p.ToString(), UriKind.Absolute));
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            _imagebox.Source = bmp;
           
            _imagebox.EndInit();
            FileInfo info = new FileInfo(p.ToString());
             name = info.Name;
            _Image_name.Content = name;
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

        

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string datacontext = "";
            if ((string)(_button_Biodiversity_data.Tag.ToString()) == "Enabled")
            {
                datacontext = "Bio";

            }
            else
            {
                if ((string)(_button_Design_ideas.Tag.ToString()) == "Enabled")
                {
                    datacontext = "Design";
                }

            }

            if (datacontext == "Bio")
            {
                if (_Data_collectionbox.Text.ToString() == "Please enter Bio-Diversity-Data" || _Data_collectionbox.Text.ToString() == "The data has been saved")
                {
                    _Data_collectionbox.Text = "Please enter Bio-Diversity-Data";
                }
                else
                {
                    SqlCeConnection conn = null;
                    _list_scroller.Visibility = Visibility.Visible;
                    string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                    string connectionString = string.Format("Data Source=" + filesPath);
                    conn = new SqlCeConnection(connectionString);
                    SqlCeCommand cmd = conn.CreateCommand();
                    conn.Open();
                    cmd.CommandText = "INSERT INTO data_associated_images (data, Image_name,image_data_timestamp) VALUES ('" + _Data_collectionbox.Text.ToString() + "','" + _Image_name.Content.ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "')";
                    _list_scroller.Items.Add(_Data_collectionbox.Text.ToString());
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    _Data_collectionbox.Text = "The data has been saved";

                }
            }
            else
                if (_Data_collectionbox.Text.ToString() == "Please enter Design-Ideas" || _Data_collectionbox.Text.ToString() == "The data has been saved")
                {
                    _Data_collectionbox.Text = "Please enter Design-Ideas";
                }
                else
                {
                    if (datacontext == "Design")
                    {
                        SqlCeConnection conn = null;
                        _list_scroller.Visibility = Visibility.Visible;
                        string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
                        string connectionString = string.Format("Data Source=" + filesPath);
                        conn = new SqlCeConnection(connectionString);
                        SqlCeCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = "INSERT INTO Ideas (Idea_content,idea_timestamp,template) VALUES ('" + _Data_collectionbox.Text.ToString() + "','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ")+"','Images'"+")";
                        _list_scroller.Items.Add(_Data_collectionbox.Text.ToString());
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        _Data_collectionbox.Text = "The data has been saved";

                    }
                }


            
        }

        private void _button_Biodiversity_data_Click(object sender, RoutedEventArgs e)
        {
            _button_Biodiversity_data.Tag = "Enabled";
            _button_Biodiversity_data.Background = Brushes.Red;
            _button_Design_ideas.Background = Brushes.Gray;
            _button_Design_ideas.Tag = "Disabled";
            _Data_collectionbox.Visibility = System.Windows.Visibility.Visible;
            surfaceButton3.Visibility = System.Windows.Visibility.Visible;
            _Data_collectionbox.Text = "Please enter Bio-Diversity-Data";
            _button_Biodiversity_data.BorderThickness = new Thickness(10);
            _button_Biodiversity_data.BorderBrush = Brushes.Black;
            _button_Design_ideas.BorderBrush = null;
            _list_scroller.Visibility = Visibility.Visible;
            _list_scroller.Items.Clear();
            SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "SELECT data, Image_name FROM data_associated_images WHERE (Image_name = N'" + name + "')";
            _list_scroller.Visibility = Visibility.Visible;
            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                SurfaceListBoxItem datafromdatabase = new SurfaceListBoxItem();
                datafromdatabase.Content = reader.GetString(0);
                datafromdatabase.AllowDrop = false;
                _list_scroller.Items.Add(datafromdatabase);
            }
            conn.Close();

        }

        private void _button_Design_ideas_Click(object sender, RoutedEventArgs e)
        {
            _button_Design_ideas.Background = Brushes.Red;
            _button_Biodiversity_data.Background = Brushes.Gray;
            _button_Design_ideas.Tag = "Enabled";
            _button_Biodiversity_data.Tag = "Disabled";
            _button_Design_ideas.BorderThickness = new Thickness(10);
            _button_Design_ideas.BorderBrush = Brushes.Black;
            _button_Biodiversity_data.BorderBrush = null;
            _Data_collectionbox.Visibility = System.Windows.Visibility.Visible;
            _Data_collectionbox.Text = "Please enter Design-Ideas";
            surfaceButton3.Visibility = System.Windows.Visibility.Visible;
            _list_scroller.Visibility = Visibility.Visible;
            _list_scroller.Items.Clear();
            SqlCeConnection conn = null;

            string filesPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NatureNetDataBase_Main.sdf");
            string connectionString = string.Format("Data Source=" + filesPath);
            conn = new SqlCeConnection(connectionString);
            SqlCeCommand cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = "SELECT Idea_content FROM Ideas WHERE (template = N'Images')";

            SqlCeDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _list_scroller.Items.Add(reader.GetString(0));
            }
            conn.Close();
        }


    }
}
