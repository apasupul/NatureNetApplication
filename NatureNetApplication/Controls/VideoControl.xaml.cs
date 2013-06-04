using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SoftwareLab.Components.YouTube;
using Vlc.DotNet.Core.Medias;
using VideoStream.UI.Events;
using SoftwareLab.Sys.WPF;
using Microsoft.Surface.Presentation.Controls;
using System;


namespace VideoStream.UI.Controls
{
    /// <summary>
    /// Interaction logic for VideoControl
    /// </summary>
    public partial class VideoControl : UserControl
    {
        #region private properties
        private bool _isExpanded = false;
        #endregion

        #region static readonlyructor / destructor
        public VideoControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                Storyboard gridSb = this.FindResource("gridOut") as Storyboard;
                Storyboard buttonSb = this.FindResource("ButtonRotationExpand") as Storyboard;
                
                if (gridSb != null)
                {
                    gridSb.Begin(this);
                    buttonSb.Begin(this);
                    _isExpanded = !_isExpanded;
                    myVlcControl.Stop();

                    myVlcControl.Media = new LocationMedia(@"http://www.youtube.com/watch?v=sfPM77TsGaA");
                    myVlcControl.Play();
                }
            }

            myVlcControl.VideoProperties.Scale = 2.0f;

            this.Unloaded += new RoutedEventHandler(Viewer_Unloaded);
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(Viewer_DataContextChanged);
        }

        void Viewer_Unloaded(object sender, RoutedEventArgs e)
        {
            myVlcControl.Stop();
        }
        
        void Viewer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Video video = DataContext as Video;
            if (video != null)
            {
                myVlcControl.Stop();

                myVlcControl.Media = new LocationMedia(video.LinkUrl);
                myVlcControl.Play();
            }
        }
        #endregion

        #region playing muting
        private void PlayButton_Down(object sender, RoutedEventArgs e)
        {
            myVlcControl.Play();
            PlayButton.Visibility = System.Windows.Visibility.Hidden;
            PauseButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void PauseButton_Down(object sender, RoutedEventArgs e)
        {
            myVlcControl.Pause();
            PauseButton.Visibility = System.Windows.Visibility.Hidden;
            PlayButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void MuteButton_Down(object sender, RoutedEventArgs e)
        {
            myVlcControl.AudioProperties.Volume = 0;
            MuteButton.Visibility = System.Windows.Visibility.Hidden;
            UnMuteButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void UnMuteButton_Down(object sender, RoutedEventArgs e)
        {
            myVlcControl.AudioProperties.Volume = 100;
            UnMuteButton.Visibility = System.Windows.Visibility.Hidden;
            MuteButton.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region expand button events
        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {
            ExpandButton_Event();
        }

        private void ExpandButton_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            ExpandButton_Event();
        }

        private void ExpandButton_Event()
        {
            Storyboard gridSb;
            Storyboard buttonSb;
            if (_isExpanded)
            {
                gridSb = this.FindResource("gridIn") as Storyboard;
                buttonSb = this.FindResource("ButtonRotationCollapse") as Storyboard;
            }
            else
            {
                gridSb = this.FindResource("gridOut") as Storyboard;
                buttonSb = this.FindResource("ButtonRotationExpand") as Storyboard;
            }

            if (gridSb != null)
            {
                gridSb.Begin(this);
                buttonSb.Begin(this);
                _isExpanded = !_isExpanded;
            }
        }
        #endregion

        private void AnimateExit()
        {
            ScatterViewItem svi = GuiHelpers.GetParentObject<ScatterViewItem>(this, false);
            if (svi != null)
            {
                IEasingFunction ease = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
                Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
                DoubleAnimation o = new DoubleAnimation(0.0, duration);
                o.Completed += new EventHandler(o_Completed);
                svi.BeginAnimation(ScatterViewItem.OpacityProperty, o);
            }
            ScatterView sv = GuiHelpers.GetParentObject<ScatterView>(svi, false);
            if (sv != null)
            {
                foreach (object obj in sv.Items)
                {
                    ScatterViewItem item = obj as ScatterViewItem;
                    if (item != null)
                    {
                        SearchResultControl ctrl = item.Content as SearchResultControl;
                        if (ctrl != null)
                        {
                            ctrl.EnableVideo(this.DataContext as Video);
                        }
                    }
                }
            }
        }

        

        void o_Completed(object sender, EventArgs e)
        {
            // alwaye detach eventes!
            // TODO: verify if the event id detached properly
            AnimationClock ani = sender as AnimationClock;
            if (ani != null)
                ani.Completed -= o_Completed;

            ScatterView sv = GuiHelpers.GetParentObject<ScatterView>(this);
            
            if (sv != null)
            {
                ScatterViewItem svi = GuiHelpers.GetParentObject<ScatterViewItem>(this, false);
                if (svi != null)
                {
                    sv.Items.Remove(svi);
                }
            }
        }

        private void btnClose_Event(object sender, RoutedEventArgs e)
        {
            AnimateExit();
            myVlcControl.Stop();
        }
    }
}
