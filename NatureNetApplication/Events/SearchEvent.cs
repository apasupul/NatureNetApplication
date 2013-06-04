using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VideoStream.UI.Events
{
    
    public delegate void SearchEventHandler(object sender, SearchEventArgs e);

    public class SearchEventArgs : RoutedEventArgs
    {
        // Fields
        private string _keyWord;
        
        public SearchEventArgs(string keyWord, RoutedEvent evt):base(evt)
        {
            _keyWord = keyWord;
           
        }

        public virtual string KeyWord
        {
            get
            {
                return _keyWord;
            }
            internal set
            {
                _keyWord = value;
            }
        }
    }
}
