using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Input;
using System.Threading;
using System.Windows.Threading;
//using SurfaceBluetoothV2.Metadata;


namespace NatureNetApplication
{
    /// <summary>
    /// A class that inherit ScatterView with Drag & Drop support.
    /// </summary>
    public class DragDropScatterView : ScatterView
    {
        public DragDropScatterView()
        {
            // Change Background to transparent, or Drag Drop hit test will by pass it if its Background is null.
            Background = Brushes.Transparent;
            AllowDrop = true;

            Loaded += new RoutedEventHandler(OnLoaded);
            Unloaded += new RoutedEventHandler(OnUnloaded);
        }

        #region Public Properties

        /// <summary>
        /// Property to identify whether the ScatterViewItem can be dragged.
        /// </summary>
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.Register(
            "AllowDrag",
            typeof(bool),
            typeof(DragDropScatterView),
            new PropertyMetadata(true/*defaultValue*/));

        /// <summary>
        /// Getter of AllowDrag AttachProperty.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetAllowDrag(DependencyObject element)
        {
            if (!(element is ScatterViewItem))
            {
                throw new InvalidOperationException(Properties.Resources.AllowDragOnlyOnScatterViewItem);
            }

            return (bool)element.GetValue(AllowDragProperty);
        }

        /// <summary>
        /// Setter of AllowDrag AttachProperty.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetAllowDrag(DependencyObject element, bool value)
        {
            if (!(element is ScatterViewItem))
            {
                throw new InvalidOperationException(Properties.Resources.AllowDragOnlyOnScatterViewItem);
            }

            element.SetValue(AllowDragProperty, value);
        }

        #endregion

        #region Private Methods

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SurfaceDragDrop.AddDropHandler(this, OnCursorDrop);
            AddHandler(ScatterViewItem.ContainerManipulationStartedEvent, new ContainerManipulationStartedEventHandler(OnManipulationStarted));
        }

       
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            SurfaceDragDrop.RemoveDropHandler(this, OnCursorDrop);
            RemoveHandler(ScatterViewItem.ContainerManipulationStartedEvent, new ContainerManipulationStartedEventHandler(OnManipulationStarted));
        }

        private void OnManipulationStarted(object sender, RoutedEventArgs args)
        {
            ScatterViewItem svi = args.OriginalSource as ScatterViewItem;
            if (svi != null && DragDropScatterView.GetAllowDrag(svi))
            {
                svi.BeginDragDrop(svi.DataContext);
            }
        }
        /// <summary>
        /// detects a drop event anywhere on the scatterview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnCursorDrop(object sender, SurfaceDragDropEventArgs args)
        {

            do_ondrop(sender, args);
            
        }

        #endregion
        /// <summary>
        /// if a image is dropped on the scatterview it creats a new image viewig box and restores the drag property of the dragged image 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void do_ondrop(object sender, SurfaceDragDropEventArgs args)
        
        {

            Thread.Sleep(TimeSpan.FromSeconds(5));
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
            SurfaceDragCursor droppingCursor = args.Cursor;
            // args.Cursor.Effects = DragDropEffects.Copy;
            LibraryBar currentbar = args.Cursor.DragSource as LibraryBar;
            if (currentbar != null)
            {
                currentbar.SetIsItemDataEnabled(args.Cursor.Data, true);
            }
            // Add dropping Item that was from another drag source.
            if (droppingCursor.CurrentTarget == this && droppingCursor.DragSource != this)
            {
                if (!Items.Contains(droppingCursor.Data))
                {
                    // Items.Add(droppingCursor.Data);
                    try
                    {
                        string[] lastPart = droppingCursor.Data.ToString().Split('.');
                        if (lastPart[lastPart.Length - 1] != "jpg")
                        {
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        return;

                    }
                    Content test = new Content(droppingCursor.Data);

                    Items.Add(test);

                    var svi = ItemContainerGenerator.ContainerFromItem(test) as ScatterViewItem;
                    if (svi != null)
                    {
                        svi.Style = (Style)Resources["LibraryContainerInScatterViewItemStyle"];
                        svi.Center = droppingCursor.GetPosition(this);
                        svi.Orientation = droppingCursor.GetOrientation(this);
                        svi.Height = 300; //((UserControl)droppingCursor.Data).Height;
                        svi.Width = 300;// ((UserControl)droppingCursor.Data).Width;
                        svi.MinHeight = 300;
                        svi.MinWidth = 300;
                        svi.MaxHeight = 1000;
                        svi.MaxWidth = 1000;
                        svi.DataContext = droppingCursor.Data.ToString();
                        svi.Tag = droppingCursor.Data.ToString();

                        svi.SetRelativeZIndex(RelativeScatterViewZIndex.Topmost);
                    }
                }
            }
                        }
                          );

        }

    }

}
