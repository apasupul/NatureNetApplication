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

namespace NatureNetApplication
{
    /// <summary>
    /// Interaction logic for Images_dropBox.xaml
    /// currently not used 
    /// </summary>
    public partial class Images_dropBox : UserControl
    {
        private string p;

        public Images_dropBox()
        {
            InitializeComponent();
        }

        public Images_dropBox(string p)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.p = p;


        }
    }
}
