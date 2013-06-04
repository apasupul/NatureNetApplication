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
using VideoStream.UI.Events;

namespace VideoStream.UI.Controls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void SurfaceButton_PreviewTouchDown(object sender, RoutedEventArgs e)
        {
            RaiseSearchEvent();
        }

        private void textSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RaiseSearchEvent();
            }
        }

        private void RaiseSearchEvent()
        {
            // Raise SearchEvent
            if (textSearch.Text.Trim() != string.Empty)
            {
                SearchEventArgs arg = new SearchEventArgs(textSearch.Text, SearchControl.SearchEvent);
                textSearch.Text = string.Empty;
                RaiseEvent(arg);
            }
        }

        #region SearchEvent
        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent("Search", RoutingStrategy.Bubble, typeof(SearchEventHandler), typeof(SearchControl));

        // Provide CLR accessors for the event
        public event SearchEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }
        #endregion
    }
}
