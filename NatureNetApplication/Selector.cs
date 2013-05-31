using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Surface.Presentation.Controls;
using System.Collections.Generic;


namespace Microsoft.Garage.Surface.ModeSelectorSample
{
    public static class Helpers
    {
        /// <summary>
        /// Finds a visual child of a given type.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="obj">The object at the root of the tree to search.</param>
        /// <returns>The visual child.</returns>
        public static T FindVisualChild<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a visual parent of a given type.
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="obj">The object at the root of the tree to search.</param>
        /// <returns>The visual parent.</returns>
        public static T FindVisualParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj == null)
            {
                return null;
            }

            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                T typed = parent as T;
                if (typed != null)
                {
                    return typed;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Gets the Visual Tree for a DependencyObject with that DependencyObject as the root.
        /// </summary>
        /// <param name="element">The root element.</param>
        /// <returns>The matching elements.</returns>
        public static IEnumerable<DependencyObject> GetVisualTree(this DependencyObject element)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = VisualTreeHelper.GetChild(element, i);

                yield return visualChild;

                foreach (var visualChildren in GetVisualTree(visualChild))
                {
                    yield return visualChildren;
                }
            }
        }
    }


    /// <summary>
    /// A mediator that forwards VerticalOffset and HorizontalOffset properties to a ScrollViewer, enabling animation of these properties.
    /// </summary>
    public class ScrollViewerOffsetMediator : FrameworkElement
    {
        /// <summary>
        /// Internal flag, don't scroll the ScrollViewer when doing an internal property update.
        /// </summary>
        private bool _Updating;

        #region ScrollViewer

        /// <summary>
        /// Gets or sets the ScrollViewer instance to forward Offset changes on to..
        /// </summary>
        /// <value>The scroll viewer.</value>
        public ScrollViewer ScrollViewer
        {
            get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
            set { SetValue(ScrollViewerProperty, value); }
        }

        /// <summary>
        /// Identifier for the ScrollViewer dependency property.
        /// </summary>
        public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register(
                "ScrollViewer",
                typeof(ScrollViewer),
                typeof(ScrollViewerOffsetMediator),
                new PropertyMetadata((sender, e) => (sender as ScrollViewerOffsetMediator).UpdateScrollViewer(e.OldValue as ScrollViewer)));

        /// <summary>
        /// Updates the scroll viewer.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        private void UpdateScrollViewer(ScrollViewer oldValue)
        {
            if (oldValue != null)
            {
                oldValue.ScrollChanged -= ScrollViewer_ScrollChanged;
            }

            if (ScrollViewer != null)
            {
                ScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                _Updating = true;
                VerticalOffset = ScrollViewer.VerticalOffset;
                HorizontalOffset = ScrollViewer.HorizontalOffset;
                _Updating = false;
            }
        }

        #endregion

        #region VerticalOffset

        /// <summary>
        /// Gets or sets the VerticalOffset property to forward to the ScrollViewer..
        /// </summary>
        /// <value>The vertical offset.</value>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Identifier for the VerticalOffset dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
                "VerticalOffset",
                typeof(double),
                typeof(ScrollViewerOffsetMediator),
                new PropertyMetadata((sender, e) => (sender as ScrollViewerOffsetMediator).UpdateVerticalOffset()));

        /// <summary>
        /// Updates the vertical offset.
        /// </summary>
        private void UpdateVerticalOffset()
        {
            if (_Updating || ScrollViewer == null)
            {
                return;
            }

            ScrollViewer.ScrollToVerticalOffset(VerticalOffset);
        }

        #endregion

        #region HorizontalOffset

        /// <summary>
        /// Gets or sets the HorizontalOffset property to forward to the ScrollViewer..
        /// </summary>
        /// <value>The Horizontal offset.</value>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Identifier for the HorizontalOffset dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(
                "HorizontalOffset",
                typeof(double),
                typeof(ScrollViewerOffsetMediator),
                new PropertyMetadata((sender, e) => (sender as ScrollViewerOffsetMediator).UpdateHorizontalOffset()));

        /// <summary>
        /// Updates the Horizontal offset.
        /// </summary>
        private void UpdateHorizontalOffset()
        {
            if (_Updating || ScrollViewer == null)
            {
                return;
            }

            ScrollViewer.ScrollToHorizontalOffset(HorizontalOffset);
        }

        #endregion

        /// <summary>
        /// Handles the ScrollChanged event of the ScrollViewer control. Updates the internal properties to reflect the changed scroll.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.ScrollChangedEventArgs"/> instance containing the event data.</param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            _Updating = true;
            VerticalOffset = e.VerticalOffset;
            HorizontalOffset = e.HorizontalOffset;
            _Updating = false;
        }
    }

    /// <summary>
    /// A ListBox which only shows the selected item, but expands to show other items when tapped. Similar to a Combo Box.
    /// </summary>
    public class ModeSelector : Control
    {
        /// <summary>
        /// A hidden button which expands the control when clicked.
        /// </summary>
        private SurfaceButton _TriggerButton;

        /// <summary>
        /// The ListBox that this control is wrapping.
        /// </summary>
        private SurfaceListBox _ListBox;

        /// <summary>
        /// How long the expand/collapse transitions should take.
        /// </summary>
        private static TimeSpan _ExpandDuration = TimeSpan.FromSeconds(.12);




        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _TriggerButton = GetTemplateChild("PART_TriggerButton") as SurfaceButton;
                _TriggerButton.Click += TriggerButton_Click;

                _ListBox = GetTemplateChild("PART_ListBox") as SurfaceListBox;
                _ListBox.LayoutUpdated += ListBox_LayoutUpdated;
                _ListBox.SelectedIndex = SelectedIndex;
                _ListBox.SelectionChanged += ListBox_SelectionChanged;
                _ListBox.PreviewMouseDown += (sender, e) => HandleClickOrTap(e.OriginalSource);

                Loaded += (sender, e) =>
                {
                    ScatterViewItem svi = this.FindVisualParent<ScatterViewItem>();
                    if (svi != null)
                    {
                        // If hosted within a ScatterViewItem, close whenever the SVI is manipulated.
                        svi.PreviewMouseLeftButtonDown += (a, b) => CloseOnParentInteraction(b.OriginalSource as FrameworkElement);
                        svi.PreviewTouchDown += (a, b) => CloseOnParentInteraction(b.OriginalSource as FrameworkElement);
                    }
                };
            }

            base.OnApplyTemplate();
        }

        /// <summary>
        /// Handles the Click event of the TriggerButton control. Expand the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void TriggerButton_Click(object sender, RoutedEventArgs e)
        {
            // This could be a good location to play some audio to provide feedback 
            IsExpanded = true;
        }

        /// <summary>
        /// Handles the SizeChanged event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void ListBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!IsExpanded)
            {
                UpdateIsExpanded(false);
            }
        }

        /// <summary>
        /// Wait until the list has been created and the default item is selected, then set the initial size and position.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ListBox_LayoutUpdated(object sender, EventArgs e)
        {
            SurfaceListBoxItem selectedItem = GetSelectedItem();

            if (selectedItem == null)
            {
                return;
            }

            _ListBox.LayoutUpdated -= ListBox_LayoutUpdated;
            UpdateIsExpanded(false);
        }

        /// <summary>
        /// Return the currently selected SurfaceListBoxItem.
        /// </summary>
        /// <returns>The currently selected SurfaceListBoxItem.</returns>
        private SurfaceListBoxItem GetSelectedItem()
        {
            return (from element in _ListBox.GetVisualTree()
                    where element is SurfaceListBoxItem
                    where (element as SurfaceListBoxItem).IsSelected == true
                    select element).FirstOrDefault() as SurfaceListBoxItem;
        }

        #region SelectedItem

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Identifier for the SelectedItem dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ModeSelector), new PropertyMetadata(null));

        #endregion

        #region SelectedIndex

        /// <summary>
        /// Gets or sets the selected index.
        /// </summary>
        /// <value>The selected index.</value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// The identifier for the SelectedIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ModeSelector), new PropertyMetadata(-1));

        #endregion

        #region ItemsSource

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Identifier for the ItemsSource property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ModeSelector), new PropertyMetadata(null));

        #endregion

        #region IsExpanded

        /// <summary>
        /// Gets or sets a value indicating whether the list is expanded and scrollable..
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Identifier for the IsExpanded dependency property/
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(ModeSelector), new PropertyMetadata(false, (sender, e) => (sender as ModeSelector).UpdateIsExpanded(true))); 

        /// <summary>
        /// Expands or collapses the control.
        /// </summary>
        /// <param name="smooth">if set to <c>true</c> animate the transition.</param>
        private void UpdateIsExpanded(bool smooth)
        {
            SurfaceListBoxItem selectedItem = GetSelectedItem();

            if (selectedItem == null)
            {
                return;
            }

            SizeChanged -= ListBox_SizeChanged;

            if (IsExpanded)
            {
                DoExpandTransition(smooth);

                if (DropDownOpened != null)
                {
                    DropDownOpened(this, EventArgs.Empty);
                }
            }
            else
            {
                DoCollapseTransition(smooth);

                if (DropDownClosed != null)
                {
                    DropDownClosed(this, EventArgs.Empty);
                }
            }

            VisualStateManager.GoToState(this, IsExpanded ? "IsExpandedState" : "NotExpandedState", true);
        }

        /// <summary>
        /// Occurs when the dropdown is opened.
        /// </summary>
        public event EventHandler DropDownOpened;

        /// <summary>
        /// Occurs when the dropdown is closed.
        /// </summary>
        public event EventHandler DropDownClosed;

        /// <summary>
        /// Does the expand transition.
        /// </summary>
        /// <param name="smooth">if set to <c>true</c> [smooth].</param>
        private void DoExpandTransition(bool smooth)
        {
            if (smooth)
            {
                IsHitTestVisible = false;
            }

            _TriggerButton.Visibility = Visibility.Collapsed;
            SurfaceListBoxItem selectedItem = GetSelectedItem();

            SurfaceScrollViewer sv = _ListBox.FindVisualChild<SurfaceScrollViewer>();
            sv.PanningMode = PanningMode.VerticalOnly;

            double topMargin = (-MaxHeight / 2) + (selectedItem.ActualHeight / 2);
            double offset = sv.VerticalOffset + topMargin;

            if (!smooth)
            {
                _ListBox.Height = MaxHeight;
                _ListBox.Margin = new Thickness(0, topMargin, 0, topMargin);
                sv.ScrollToVerticalOffset(offset);
                return;
            }

            Storyboard storyboard = new Storyboard();

            DoubleAnimation height = new DoubleAnimation();
            Storyboard.SetTarget(height, _ListBox);
            Storyboard.SetTargetProperty(height, new PropertyPath(ListBox.HeightProperty));
            height.To = MaxHeight;
            height.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            height.Duration = _ExpandDuration;
            storyboard.Children.Add(height);

            ThicknessAnimation margin = new ThicknessAnimation();
            Storyboard.SetTarget(margin, _ListBox);
            Storyboard.SetTargetProperty(margin, new PropertyPath(ListBox.MarginProperty));
            margin.To = new Thickness(0, topMargin, 0, topMargin);
            margin.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            margin.Duration = _ExpandDuration;
            storyboard.Children.Add(margin);

            DoubleAnimation verticalOffset = new DoubleAnimation();
            Storyboard.SetTarget(verticalOffset, this.FindVisualChild<ScrollViewerOffsetMediator>());
            Storyboard.SetTargetProperty(verticalOffset, new PropertyPath(ScrollViewerOffsetMediator.VerticalOffsetProperty));
            verticalOffset.To = offset;
            verticalOffset.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            verticalOffset.Duration = _ExpandDuration;
            storyboard.Children.Add(verticalOffset);

            ScrollBar scrollbar = this.FindVisualChild<ScrollBar>();
            if (MaxHeight < ActualHeight * _ListBox.Items.Count && scrollbar != null)
            {
                DoubleAnimation scroll = new DoubleAnimation();
                Storyboard.SetTarget(scroll, scrollbar);
                Storyboard.SetTargetProperty(scroll, new PropertyPath(UIElement.OpacityProperty));
                scroll.To = 1;
                scroll.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
                scroll.Duration = _ExpandDuration;
                storyboard.Children.Add(scroll);
            }

            height.Completed += (sender, e) =>
            {
                IsHitTestVisible = true;
                SizeChanged += ListBox_SizeChanged;
            };

            storyboard.Begin(this, true);
        }

        /// <summary>
        /// Does the collapse transition.
        /// </summary>
        /// <param name="smooth">if set to <c>true</c> [smooth].</param>
        private void DoCollapseTransition(bool smooth)
        {
            if (smooth)
            {
                IsHitTestVisible = false;
            }

            _TriggerButton.Visibility = Visibility.Visible;

            // Find the selected list box item.
            SurfaceListBoxItem selectedItem = GetSelectedItem();

            if (selectedItem == null)
            {
                return;
            }

            SurfaceScrollViewer sv = _ListBox.FindVisualChild<SurfaceScrollViewer>();
            double offset = selectedItem.TransformToVisual(sv).Transform(new Point()).Y + sv.VerticalOffset;
            sv.PanningMode = PanningMode.None;

            if (!smooth)
            {
                _ListBox.Height = selectedItem.ActualHeight;
                _ListBox.Margin = new Thickness();
                sv.ScrollToVerticalOffset(offset);
                SizeChanged += ListBox_SizeChanged;
                return;
            }

            Storyboard storyboard = new Storyboard();

            DoubleAnimation height = new DoubleAnimation();
            Storyboard.SetTarget(height, _ListBox);
            Storyboard.SetTargetProperty(height, new PropertyPath(ListBox.HeightProperty));
            height.To = selectedItem.ActualHeight;
            height.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            height.Duration = _ExpandDuration;
            storyboard.Children.Add(height);

            ThicknessAnimation margin = new ThicknessAnimation();
            Storyboard.SetTarget(margin, _ListBox);
            Storyboard.SetTargetProperty(margin, new PropertyPath(ListBox.MarginProperty));
            margin.To = new Thickness();
            margin.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            margin.Duration = _ExpandDuration;
            storyboard.Children.Add(margin);

            DoubleAnimation verticalOffset = new DoubleAnimation();
            Storyboard.SetTarget(verticalOffset, this.FindVisualChild<ScrollViewerOffsetMediator>());
            Storyboard.SetTargetProperty(verticalOffset, new PropertyPath(ScrollViewerOffsetMediator.VerticalOffsetProperty));
            verticalOffset.To = offset;
            verticalOffset.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
            verticalOffset.Duration = _ExpandDuration;
            storyboard.Children.Add(verticalOffset);

            ScrollBar scrollbar = this.FindVisualChild<ScrollBar>();
            if (scrollbar != null)
            {
                DoubleAnimation scroll = new DoubleAnimation();
                Storyboard.SetTarget(scroll, scrollbar);
                Storyboard.SetTargetProperty(scroll, new PropertyPath(UIElement.OpacityProperty));
                scroll.To = 0;
                scroll.EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut };
                scroll.Duration = _ExpandDuration;
                storyboard.Children.Add(scroll);
            }

            height.Completed += (sender, e) =>
            {
                IsHitTestVisible = true;
                SizeChanged += ListBox_SizeChanged;
            };

            storyboard.Begin(this, true);
        }

        #endregion

        /// <summary>
        /// Handles the SelectionChanged event of the ListBox control. Collapses the ListBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedIndex = _ListBox.SelectedIndex;
            SelectedItem = _ListBox.SelectedItem;

            if (!IsExpanded)
            {
                UpdateIsExpanded(false);
            }
            else
            {
                // This could be a good spot to play audio that provides 
                // feedback that the selection just changed. 
                IsExpanded = false;
            }

            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when when the ListBox selection changes.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        /// <summary>
        /// Handles the click or tap.
        /// </summary>
        /// <param name="originalSource">The original source.</param>
        private void HandleClickOrTap(object originalSource)
        {
            if (IsExpanded)
            {
                ListBoxItem item = originalSource as ListBoxItem;
                if (item == null)
                {
                    item = (originalSource as FrameworkElement).FindVisualParent<ListBoxItem>();
                }

                if (item != null && item.IsSelected)
                {
                    // This could be a good spot to play audio that provides 
                    // feedback that the list is collapsing.
                    IsExpanded = false;
                }

                // If some other item is tapped, don't do anything. SelectionChanged will fire and handle it.
                return;
            }

            // Expand when the list is tapped when it's closed.
            IsExpanded = true;
        }

        /// <summary>
        /// If there is any interaction on a parent SVI, close this list.
        /// </summary>
        /// <param name="parent">The parent.</param>
        private void CloseOnParentInteraction(FrameworkElement parent)
        {
            if (parent.FindVisualParent<ModeSelector>() == null)
            {
                IsExpanded = false;
            }
        }
    }
}
