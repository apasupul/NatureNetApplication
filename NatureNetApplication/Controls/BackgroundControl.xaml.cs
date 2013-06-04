using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FluidKit.Controls;

namespace VideoStream.UI.Controls
{
    /// <summary>
    /// Interaction logic for BackgroundControl
    /// </summary>
    public partial class BackgroundControl : UserControl
    {
        #region private propertiies
        private string _backItem = "image1";
		private string _frontItem = "image2";
        private string _newImage = "";
        private static Random _randomizer = new Random();
        
        #endregion

        #region public properties
        public string NewImage
        {
            get { return _newImage; }
            set 
            {
                try
                {
                    _newImage = value;
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(_newImage);
                    img.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();

                    if (_backItem == "image1")
                        image1.Source = img;
                    else
                        image2.Source = img;

                    PlayRandomAnimation();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message, "ERROR in NewImage");
                }
            }
        }
        #endregion

        #region ctor
        public BackgroundControl()
		{
			InitializeComponent();
            Loaded += BackgroundControl_Loaded;

            // TODO: elaborate a way to preload animations to avoid first time slow
		}
        #endregion

        #region control loaded
        private void BackgroundControl_Loaded(object sender, RoutedEventArgs e)
		{
			transContainer.TransitionCompleted += transContainer_TransitionCompleted;
		}
        #endregion

        #region transition completed
        private void transContainer_TransitionCompleted(object sender, EventArgs e)
		{
			SwapFrontAndBack();
		}
        #endregion

        #region play animations
        private void PlayRandomAnimation()
        {
            // get a random animation 
            int ani = _randomizer.Next(1, 5);
            int option = 0;
            Direction dir = Direction.BottomToTop;
            GenieEffectType effect = GenieEffectType.OutOfLamp;
            switch (ani)
            {
                case 1:
                    option = _randomizer.Next(1, 3);
                    if (option == 1)
                        effect = GenieEffectType.IntoLamp;
                    else
                        effect = GenieEffectType.OutOfLamp;
                    PlayGenie(effect);
                    break;
                case 2:
                    option = _randomizer.Next(1, 5);
                    if (option == 1)
                        dir = Direction.BottomToTop;
                    else if (option == 2)
                        dir = Direction.LeftToRight;
                    else if (option == 3)
                        dir = Direction.RightToLeft;
                    else if (option == 4)
                        dir = Direction.TopToBottom;
                    PlayCube(dir);
                    break;
                case 3:
                    option = _randomizer.Next(1, 3);
                    if (option == 2)
                        dir = Direction.LeftToRight;
                    else if (option == 3)
                        dir = Direction.RightToLeft;
                    PlayFlip(dir);
                    break;
                case 4:
                    option = _randomizer.Next(1, 5);
                    if (option == 1)
                        dir = Direction.BottomToTop;
                    else if (option == 2)
                        dir = Direction.LeftToRight;
                    else if (option == 3)
                        dir = Direction.RightToLeft;
                    else if (option == 4)
                        dir = Direction.TopToBottom;
                    PlaySlide(dir);
                    break;
                default:
                    option = _randomizer.Next(1, 3);
                    if (option == 1)
                        effect = GenieEffectType.IntoLamp;
                    else
                        effect = GenieEffectType.OutOfLamp;
                    PlayGenie(effect);
                    break;
            }
        }

        private void PlayGenie(GenieEffectType effect)
		{
			GenieTransition transition = Resources["GenieTransition"] as GenieTransition;
            transition.EffectType = effect;

			transContainer.Transition = transition;
			transContainer.ApplyTransition(_frontItem, _backItem);
		}

		private void PlayCube(Direction dir)
		{
			CubeTransition transition = Resources["CubeTransition"] as CubeTransition;
            
            transition.Rotation = dir;
            
			transContainer.Transition = transition;
			transContainer.ApplyTransition(_frontItem, _backItem);
		}

		private void PlayFlip(Direction dir)
		{
			FlipTransition transition = Resources["FlipTransition"] as FlipTransition;
            transition.Rotation = dir;// Direction.LeftToRight;
            
			transContainer.Transition = transition;
			transContainer.ApplyTransition(_frontItem, _backItem);
		}

		private void PlaySlide(Direction dir)
		{
			SlideTransition transition = Resources["SlideTransition"] as SlideTransition;
            transition.Direction = dir;

			transContainer.Transition = transition;
			transContainer.ApplyTransition(_frontItem, _backItem);
		}
        #endregion

        #region swap front back
        private void SwapFrontAndBack()
		{
			string temp = _frontItem;
			_frontItem = _backItem;
			_backItem = temp;
        }
        #endregion
    }
}
