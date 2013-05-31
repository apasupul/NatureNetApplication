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

namespace Microsoft.Garage.Surface.ModeSelectorSample
{
    /// <summary>
    /// Interaction logic for selector.xaml
    /// </summary>
    public partial class selector : UserControl
    {
        public selector()
        {
            InitializeComponent();
            _ModeSelector.ItemsSource = new string[]
                                            {
                                                "Option 1",
                                                "Option 2",
                                                "Option 3",
                                                "Option 4",
                                                "Option 5",
                                                "Option 6"
                                            };

            _ModeSelector.SelectionChanged += ModeSelectorSelectionChanged;
        }
        void ModeSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox source = e.OriginalSource as ListBox;
            if (source != null)
            {
                _Results.Text = source.SelectedItem + " Selected";
            }
        }
    }
}
