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
    /// Interaction logic for Errorbox.xaml
    /// Cna be used to display any usefull error messages on screen or to catch exeptions accepsts 2 parameters to display
    /// </summary>
    public partial class Errorbox : UserControl
    {
        public Errorbox()
        {
            InitializeComponent();
        }

        public Errorbox(String generalmsg, String errormessages, String generealerrormessages)
        {
            InitializeComponent();
            generalmessages.Text = generalmsg;
            Errormessages.Text = errormessages;
            generalError.Text = generealerrormessages;
            
        }


    }
}
