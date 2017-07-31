using System.Windows;
using System.Windows.Controls;


namespace PZone.Views.Pages
{
    /// <summary>
    /// Interaction logic for Publisher.xaml
    /// </summary>
    public partial class Publisher : Page
    {
        public Publisher()
        {
            InitializeComponent();
        }
        

        private void OpenBuildJsWindow(object sender, RoutedEventArgs e)
        {
            new BuildJSWindow().Show();
        }


        private void OpenWebResourcesWindow(object sender, RoutedEventArgs e)
        {
            new WebResourcesWindow().Show();
        }

        private void OpenWorkflowLogAnalizerWindow(object sender, RoutedEventArgs e)
        {
            new WorkflowLogAnalizerWindow().Show();
        }
    }
}