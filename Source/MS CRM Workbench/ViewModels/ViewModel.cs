using System.ComponentModel;
using System.Windows;


namespace PZone.ViewModels
{
    public abstract class ViewModel : ObservableObject
    {
        private bool _busy;


        public static bool DesignMode { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());


        public bool Busy
        {
            get { return _busy; }
            set { SetProperty(ref _busy, value); }
        }
    }
}