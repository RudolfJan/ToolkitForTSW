using System;
using System.Windows;

namespace ToolkitForTSW
{
    /// <summary>
    /// Interaction logic for ThumbNailButton.xaml
    /// </summary>
    ///
    /// https://www.stevefenton.co.uk/2012/09/wpf-bubbling-a-command-from-a-child-view/
    ///  https://stackoverflow.com/questions/3067617/raising-an-event-on-parent-window-from-a-user-control-in-net-c-sharp


    public partial class ThumbNailButton
        {
        public CScreenshot Screenshot
            {
            get { return (CScreenshot) GetValue(ScreenshotProperty); }
            set { SetValue(ScreenshotProperty, value); }
            }

        public static readonly DependencyProperty ScreenshotProperty =
            DependencyProperty.Register("Screenshot", typeof(CScreenshot), typeof(ThumbNailButton),
                new PropertyMetadata(null));

        public new Int32 Height
            {
            get { return (Int32) GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
            }

        public new static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(Int32), typeof(ThumbNailButton));

        public new Int32 Width
            {
            get { return (Int32) GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
            }

        public new static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(Int32), typeof(ThumbNailButton));

        public ThumbNailButton()
            {
            InitializeComponent();
            }

        public static readonly RoutedEvent MyCustomEvent = EventManager.RegisterRoutedEvent(
            "Click", // Event name
            RoutingStrategy.Bubble, // Bubble means the event will bubble up through the tree
            typeof(RoutedEventHandler), // The event type
            typeof(ThumbNailButton)); // Belongs to ThumbNailButton

        public event RoutedEventHandler MyCustomEventHandler
        {
            add { AddHandler(MyCustomEvent, value); }
            remove { RemoveHandler(MyCustomEvent, value); }
            }

        private void OnThumbNailPartClicked(Object Sender, RoutedEventArgs E)
            {
            var MyRoutedEventArgs = new RoutedEventArgs(MyCustomEvent);
        RaiseEvent(MyRoutedEventArgs);
        }
    }
    }
