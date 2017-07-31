using System.Windows;
using System.Windows.Media;


namespace PZone.Controls
{
    public class TextBox : System.Windows.Controls.TextBox
    {
        private string _placeholder;
        private Brush _commonColor;
        private readonly Brush _placeholderColor = (Brush)new BrushConverter().ConvertFrom("#FFCFCFCF");


        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value;
                _commonColor = Foreground;
                SetPlaceholder();
            }
        }


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.Name != "Text")
                return;
            if (!IsFocused && string.IsNullOrWhiteSpace(e.NewValue?.ToString()))
                SetPlaceholder();
            else if (!string.IsNullOrWhiteSpace(e.NewValue?.ToString()) && !e.NewValue.Equals(Placeholder))
            {
                ClearPlaceholder();
            }
        }


        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Text == Placeholder)
                ClearPlaceholder();
            base.OnGotFocus(e);
        }


        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Text))
                SetPlaceholder();
            base.OnLostFocus(e);
        }


        private void SetPlaceholder()
        {
            Text = Placeholder;
            Foreground = _placeholderColor;
        }


        private void ClearPlaceholder()
        {
            if (Text == Placeholder)
                Text = string.Empty;
            Foreground = _commonColor;
        }
    }
}
