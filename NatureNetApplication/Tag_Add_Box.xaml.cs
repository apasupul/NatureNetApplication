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
using Microsoft.Surface.Presentation;
using System.Collections.ObjectModel;

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for Tag_Add_Box.xaml
    /// </summary>
    public partial class Tag_Add_Box : UserControl
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
        SurfaceWindow1 net = new SurfaceWindow1();

        public Tag_Add_Box()
        {
            InitializeComponent();
        }

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            if ((Tag_box_Content.Text == ""))
            {
                Tag_box_Content.Text = "please enter a Tag name:";

            }
            else
            {
                String[] myListItemArray = new String[Images_drop_window.Items.Count];
                Images_drop_window.Items.CopyTo(myListItemArray, 0);
                foreach (String myItem in myListItemArray)
                { 
                }
               // SurfaceWindow1.tW1._lo
               // SurfaceWindow1.tW1.Tagloadbox.Items.Add(Tag_box_Content.Text);
            }

        }
        private void Push_Image_container_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            object neededdata = e.Cursor.Data;
        
         
            Content test = e.Cursor.Data as Content;
           
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
           
            SurfaceDragCursor droppingCursor = e.Cursor;
            if (Names == null)
            {
                Names.Add(test.p.ToString());
            }
            if (!Names.Contains(test.p.ToString()))
            {
                Names.Add(test.p.ToString());

            }
            Images_drop_window.DataContext = this;
            Images_drop_window.Items.Add(test.p.ToString());
            //Images_drop_window.ItemsSource = names;
            e.Handled = true;
           

        }

        private void ScatterViewItem_Drop(object sender, DragEventArgs e)
        {

        }

        private void surfaceButton2_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
