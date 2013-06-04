using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using SoftwareLab.Components.YouTube;
using SoftwareLab.Sys.Collections.ObjectModel;
using SoftwareLab.Sys.WPF;
using VideoStream.UI.Events;
using System.Collections;

namespace VideoStream.UI.Controls
{
    /// <summary>
    /// Interaction logic for SearchResultControl.xaml
    /// </summary>
    public partial class SearchResultControl : UserControl//, VideoStream.UI.Interfaces.IContent
    {
        public SearchResultControl()
        {
            InitializeComponent();

            SurfaceDragDrop.AddPreviewQueryTargetHandler(container, Container_PreviewQueryTarget);
            this.Loaded += new RoutedEventHandler(SearchResultControl_Loaded);
            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(SearchResultControl_DataContextChanged);

            // TODO: elaborate a way to manage no result!
        }

        void SearchResultControl_Loaded(object sender, RoutedEventArgs ea)
        {
            Size targetSize = new Size(270, 270);
            var svi = GuiHelpers.GetParentObject<ScatterViewItem>(this, false);
            
            if (svi != null)
            {
                // Easing function provide a more natural animation
                IEasingFunction ease = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
                Duration duration = new Duration(TimeSpan.FromMilliseconds(1500));
                DoubleAnimation o = new DoubleAnimation(0.0, 1.0, duration);

                // Set the size manually, otherwise once the animation is removed the size will revert back to the minimum size
                // Since animation has higher priority for DP's, this setting won't have effect until the animation is removed
                svi.Width = targetSize.Width;
                svi.Height = targetSize.Height;
                svi.MinWidth = targetSize.Width;
                svi.MinHeight = targetSize.Height;
                svi.MaxHeight = 800;

                // questo particolare scatterview non puo' essere ridimensionato
                svi.CanScale = false;

                svi.BeginAnimation(ScatterViewItem.OpacityProperty, o);
            }
        }

        private void Container_PreviewQueryTarget(object sender, QueryTargetEventArgs e)
        {
            //allows only to item contained in the library container of being dropped in
            //LibraryContainer bar = (LibraryContainer)sender;
            bool contained = false;
            foreach (object o in container.ItemsSource)
            {
                if (o is Video)
                {
                    if (e.Cursor.Data as Video != null && (e.Cursor.Data as Video).Equals(o))
                    {
                        contained = true;
                        break;
                    }
                }
            }

            if (contained == false)
            {
                e.UseDefault = false;
                e.ProposedTarget = null;
                e.Handled = true;
            }
        }

        public void AnimateExit()
        {
            var svi = GuiHelpers.GetParentObject<ScatterViewItem>(this, false);
            if (svi != null)
            {
                IEasingFunction ease = new BackEase { EasingMode = EasingMode.EaseOut, Amplitude = 0.3 };
                Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
                DoubleAnimation o = new DoubleAnimation(0.0, duration);
                o.Completed += new EventHandler(o_Completed);
                svi.BeginAnimation(ScatterViewItem.OpacityProperty, o);
            }
        }

        void o_Completed(object sender, EventArgs e)
        {
            // alwaye detach eventes!
            // TODO: verify if the event id detached properly
            AnimationClock ani = sender as AnimationClock;
            if (ani != null)
                ani.Completed -= o_Completed;

            ScatterViewItem svi = GuiHelpers.GetParentObject<ScatterViewItem>(this);
            if (svi != null)
            {
                ScatterView sv = GuiHelpers.GetParentObject<ScatterView>(svi);
                if (sv != null)
                    sv.Items.Remove(svi);
            }
        }

        public void EnableVideo(Video video)
        {
            if (video != null)
                container.SetIsItemDataEnabled(video, true);
        }

        private void btnClose_Event(object sender, RoutedEventArgs e)
        {
            AnimateExit();
        }
       
    }
}
