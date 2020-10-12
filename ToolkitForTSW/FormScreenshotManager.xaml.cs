using System;
using System.Windows;
using System.Windows.Controls;

//	https://github.com/mirzaevolution/ThumbnailSharp

namespace ToolkitForTSW
    {
    /// <summary>
    /// Interaction logic for FormScreenshotManager.xaml
    /// </summary>
    public partial class FormScreenshotManager
        {
        CScreenshotManager ScreenshotManager { get; set; }

        public FormScreenshotManager()
            {
            InitializeComponent();
            var WinHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.85;
            var AllowedHeightFactor = 0.8;
            if (ThumbNailScrollView.Height / WinHeight > AllowedHeightFactor)
                {
                ThumbNailScrollView.Height = ThumbNailScrollView.MaxHeight *
                                             ThumbNailScrollView.MaxHeight / WinHeight;
                }

            ScreenshotManager = new CScreenshotManager();
            DataContext = ScreenshotManager;
            SetControlStates();
            }

        private void SetControlStates()
            {
            NextButton.IsEnabled = ScreenshotManager.DisplayRangeOffset <
                                   ScreenshotManager.ScreenshotList.Count;
            // PreviousButton.IsEnabled = ScreenshotManager.DisplayRangeOffset > ScreenshotManager.DisplayRangeCount;
            }

        private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
            {
            Close();
            }

        private void OnPreviousButtonClicked(Object Sender, RoutedEventArgs E)
            {
            if (ScreenshotManager.DisplayRangeOffset > ScreenshotManager.DisplayRangeCount)
                {
                ScreenshotManager.DisplayRangeOffset -= ScreenshotManager.DisplayRangeCount;
                ScreenshotManager.GetDisplayRange();
                SetControlStates();
                }
            }

        private void OnNextButtonClicked(Object Sender, RoutedEventArgs E)
            {
            if (ScreenshotManager.DisplayRangeOffset < ScreenshotManager.ScreenshotList.Count)
                {
                ScreenshotManager.DisplayRangeOffset += ScreenshotManager.DisplayRangeCount;
                ScreenshotManager.GetDisplayRange();
                SetControlStates();
                }
            }

        private void OnThumbNailButtonClicked(Object Sender, RoutedEventArgs E)
            {
            // Go down starting at the button to retrieve the Image path

            CScreenshot Screenshot = ((ThumbNailButton) Sender).Screenshot;
            if (Screenshot != null)
                {
                var Form = new FormScreenshot(Screenshot);
                Form.Show();
                }

            SetControlStates();
            }

        private void OnFirstButtonClicked(Object Sender, RoutedEventArgs E)
            {
            ScreenshotManager.DisplayRangeOffset = 0;
        if (ScreenshotManager.DisplayRangeOffset < ScreenshotManager.ScreenshotList.Count)
            {
            ScreenshotManager.GetDisplayRange();
            SetControlStates();
            }
        }

        private void OnLastButtonClicked(Object Sender, RoutedEventArgs E)
        {
        ScreenshotManager.DisplayRangeOffset = (ScreenshotManager.ScreenShotTotalPages)*ScreenshotManager.DisplayRangeCount;
        if (ScreenshotManager.DisplayRangeOffset < ScreenshotManager.ScreenshotList.Count)
            {
            ScreenshotManager.GetDisplayRange();
            SetControlStates();
            }
        }
    }
    }
