using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Microsoft.Surface.Presentation.Controls;
using SoftwareLab.Components.Bing;
using SoftwareLab.Components.YouTube;
using SoftwareLab.Sys.Collections.ObjectModel;
using VideoStream.UI.Events;
using Vlc.DotNet.Core;


namespace VideoStream.UI.Controls
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        private VideoDispenser videoDispenser = new VideoDispenser();
        private BingImageSearch bingImageSearch = new BingImageSearch();
        private static Random _randomizer = new Random();
        private ResourceDictionary LibrayStiles;

        public MainControl()
        {
            #region video player setup
            // Set libvlc.dll and libvlccore.dll directory path
            VlcContext.LibVlcDllsPath = CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;// @"C:\Projets\vlc-1.2.0-pre1-20111109-0006";

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = CommonStrings.PLUGINS_PATH_DEFAULT_VALUE_AMD64;//@"C:\Projets\vlc-1.2.0-pre1-20111109-0006\plugins";

            /* Setting up the configuration of the VLC instance.
             * You can use any available command-line option using the AddOption function (see last two options). 
             * A list of options is available at 
             *     http://wiki.videolan.org/VLC_command-line_help
             * for example. */

            // Ignore the VLC configuration file
            VlcContext.StartupOptions.IgnoreConfig = true;

            VlcContext.StartupOptions.ShowLoggerConsole = false;
            VlcContext.StartupOptions.ScreenSaverEnabled = false;

            // Enable file based logging
            VlcContext.StartupOptions.LogOptions.LogInFile = true;

            // Shows the VLC log console (in addition to the applications window)
            VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;

            // Set the log level for the VLC instance
            VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.None;

            // Disable showing the movie file name as an overlay
            VlcContext.StartupOptions.AddOption("--no-video-title-show");

            // Pauses the playback of a movie on the last frame
            //VlcContext.StartupOptions.AddOption("--play-and-pause");

            // Initialize the VlcContext
            VlcContext.Initialize();
            #endregion


            //#region vlc init with debug
            //// Set libvlc.dll and libvlccore.dll directory path
            //VlcContext.LibVlcDllsPath = CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;// @"C:\Projets\vlc-1.2.0-pre1-20111109-0006";

            //// Set the vlc plugins directory path
            //VlcContext.LibVlcPluginsPath = CommonStrings.LIBVLC_DLLS_PATH_DEFAULT_VALUE_AMD64;// @"C:\Projets\vlc-1.2.0-pre1-20111109-0006\plugins";

            ///* Setting up the configuration of the VLC instance.
            // * You can use any available command-line option using the AddOption function (see last two options). 
            // * A list of options is available at 
            // *     http://wiki.videolan.org/VLC_command-line_help
            // * for example. */

            //// Ignore the VLC configuration file
            //VlcContext.StartupOptions.IgnoreConfig = true;

            //// Enable file based logging
            //VlcContext.StartupOptions.LogOptions.LogInFile = true;

            //// Shows the VLC log console (in addition to the applications window)
            //VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = true;

            //// Set the log level for the VLC instance
            //VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;

            //// Disable showing the movie file name as an overlay
            //VlcContext.StartupOptions.AddOption("--no-video-title-show");

            //// Pauses the playback of a movie on the last frame
            ////VlcContext.StartupOptions.AddOption("--play-and-pause");

            //// Initialize the VlcContext
            //VlcContext.Initialize();
            //#endregion


            InitializeComponent();

            this.Unloaded += new RoutedEventHandler(MainControl_Unloaded);

            LibrayStiles = new ResourceDictionary();
            LibrayStiles.Source = new Uri("/VideoStream;component/Styles/LibraryDictionary.xaml", UriKind.RelativeOrAbsolute);


            videoDispenser.DownloadCompleted += new VideosDownloadCompletedEventHandler(videoDispenser_DownloadCompleted);
            bingImageSearch.DownloadCompleted += new BingImageDownloadCompletedEventHandler(bingImageSearch_DownloadCompleted);

        }

        void MainControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //VlcContext.CloseAll();
        }

        void bingImageSearch_DownloadCompleted(object sender, BingImageDownloadCompletedEventArgs e)
        {
            ContentGrid.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                    (System.Action)(() =>
                    {
                        backgroundControl.NewImage = e.DownloadedBingImage.MediaFilePath;
                    }));
        }

        void videoDispenser_DownloadCompleted(object sender, VideosDownloadCompletedEventArgs e)
        {


        }

        private void Search_Event(object sender, SearchEventArgs e)
        {
            // search for videos on YouTube
            string a = e.KeyWord;
            ThreadSafeObservableCollection<Video> list = new ThreadSafeObservableCollection<Video>();
            videoDispenser.LoadVideosAsync(list, e.KeyWord, 10, 1);

            // result is added to ScatterView
            SearchResultControl ctrl = new SearchResultControl();
            //ctrl.RemoveContent += new RemoveContentHandler(RemoveContent);
            ctrl.DataContext = list;
            ScatterViewItem svi = new ScatterViewItem();
            svi.Style = (Style)LibrayStiles["LibraryContainerInScatterViewItemStyle"];
            svi.Content = ctrl;
            svi.Center = sviSearch.ActualCenter;
            scatterContainer.Items.Add(svi);

            // svi created near seach control and thrown away
            Storyboard stb = new Storyboard();
            PointAnimation moveCenter = new PointAnimation();
            Point endPoint = new Point(_randomizer.Next(0, 1500), _randomizer.Next(0, 1000));
            moveCenter.From = svi.Center;
            moveCenter.To = endPoint;
            moveCenter.Duration = new Duration(TimeSpan.FromSeconds(1.0));
            moveCenter.FillBehavior = FillBehavior.Stop;
            stb.Children.Add(moveCenter);
            Storyboard.SetTarget(moveCenter, svi);
            Storyboard.SetTargetProperty(moveCenter, new PropertyPath(ScatterViewItem.CenterProperty));
            stb.Begin(this);
            svi.Center = endPoint;

            // serach request to bing image to change the background accordingly with the serach
            bingImageSearch.DownloadImagesAsync(Guid.NewGuid(), a);
        }

        

        public void CloseMainControl()
        {
            // Close the context. 
            VlcContext.CloseAll();

            // Remove handlers for window availability events
            
        }

        #region surface handlers
        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
            //if (removableDeviceWatcher == null)
            //{
            //    removableDeviceWatcher = new RemovableDeviceWatcher(RemovableDeviceEventType.Any);
            //    removableDeviceWatcher.OnDeviceConnected += new RemovableDeviceWatcher.DeviceConnectedEventHandler(removableDeviceWatcher_OnDeviceConnected);
            //    removableDeviceWatcher.OnDeviceRemoved += new RemovableDeviceWatcher.DevicRemovedEventHandler(removableDeviceWatcher_OnDeviceRemoved);
            //}
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowNoninteractive(object sender, EventArgs e)
        {
            VlcContext.CloseAll();
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
            //if (removableDeviceWatcher != null)
            //{
            //    removableDeviceWatcher.OnDeviceConnected -= new RemovableDeviceWatcher.DeviceConnectedEventHandler(removableDeviceWatcher_OnDeviceConnected);
            //    removableDeviceWatcher.OnDeviceRemoved -= new RemovableDeviceWatcher.DevicRemovedEventHandler(removableDeviceWatcher_OnDeviceRemoved);
            //    removableDeviceWatcher.Dispose();
            //}
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnWindowUnavailable(object sender, EventArgs e)
        {
            VlcContext.CloseAll();
            //if (removableDeviceWatcher != null)
            //{
            //    removableDeviceWatcher.OnDeviceConnected -= new RemovableDeviceWatcher.DeviceConnectedEventHandler(removableDeviceWatcher_OnDeviceConnected);
            //    removableDeviceWatcher.OnDeviceRemoved -= new RemovableDeviceWatcher.DevicRemovedEventHandler(removableDeviceWatcher_OnDeviceRemoved);
            //    removableDeviceWatcher.Dispose();
            //}
        }
        #endregion

    }
}
