using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace PZone.Controls
{
    public class PublishButton : Button
    {
        private static readonly DependencyProperty _statusProperty = DependencyProperty.Register("Status", typeof(WebResourceStatus), typeof(PublishButton), new UIPropertyMetadata(default(WebResourceStatus), OnSetStatusValue));

        private static readonly DependencyProperty _statusTextProperty = DependencyProperty.Register("StatusText", typeof(string), typeof(PublishButton), new PropertyMetadata(default(string)));
        private static Brush _readyColor;
        private static readonly Brush _changesColor = Brushes.LightCoral;
        private const string CHANGES_TOOLTIP = "Веб-ресурс изменился.";
        private const string READY_TEXT = "Publish";
        private const string BUSY_TEXT = "Process...";
        private const string BUSY_TOOLTIP = "Идет публикация веб-ресурса.";
        private const string ERROR_TEXT = "Error";


        public WebResourceStatus Status
        {
            get { return (WebResourceStatus)GetValue(_statusProperty); }
            set { SetValue(_statusProperty, value); }
        }


        public string StatusText
        {
            get { return (string)GetValue(_statusTextProperty); }
            set { SetValue(_statusTextProperty, value); }
        }


        public PublishButton()
        {
            Content = READY_TEXT;
            //this.DataContext = this;
        }


        private static void OnSetStatusValue(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var button = (PublishButton)o;
            var status = (WebResourceStatus)e.NewValue;
            switch (status)
            {
                case WebResourceStatus.Changes:
                    if (_readyColor == null)
                        _readyColor = button.Background;
                    button.Background = _changesColor;
                    button.ToolTip = CHANGES_TOOLTIP;
                    break;
                case WebResourceStatus.Busy:
                    button.Content = BUSY_TEXT;
                    button.ToolTip = BUSY_TOOLTIP;
                    break;
                case WebResourceStatus.Error:
                    button.Content = ERROR_TEXT;
                    button.ToolTip = button.Status;
                    break;
                case WebResourceStatus.Ready:
                    button.Content = READY_TEXT;
                    button.Background = _readyColor;
                    button.ToolTip = null;
                    break;
            }
        }
    }
}