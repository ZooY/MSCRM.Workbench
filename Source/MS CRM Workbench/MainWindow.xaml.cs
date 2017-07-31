using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;


namespace PZone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //var watcher = new FileSystemWatcher
            //{
            //    Path = "C:\\Projects\\CRM\\CRM\\Source",
            //    IncludeSubdirectories = true
            //};
            //watcher.Changed += CheckChanged;
            //watcher.Created += CheckChanged;
            ////watcher.Deleted += CheckChanged;
            //watcher.Renamed += CheckChanged;
            //watcher.EnableRaisingEvents = true;
        }


//        private void CheckChanged(object sender, FileSystemEventArgs e)
//        {
//            if (!e.FullPath.EndsWith(".js") && !e.FullPath.EndsWith(".css") && !e.FullPath.EndsWith(".html"))
//                return;
//            SetChanged(File1, Publish1, e);
//            SetChanged(File2, Publish2, e);
//            SetChanged(File3, Publish3, e);
//            SetChanged(File4, Publish4, e);
//            SetChanged(File5, Publish5, e);
//            SetChanged(File6, Publish6, e);
//            SetChanged(File7, Publish7, e);
//            SetChanged(File8, Publish8, e);
//            SetChanged(File9, Publish9, e);
//            SetChanged(File10, Publish10, e);
//            SetChanged(File11, Publish11, e);
//            SetChanged(File12, Publish11, e);
//        }


//        private static void SetChanged(TextBox fileControl, Button publishControl, FileSystemEventArgs e)
//        {
//            fileControl.Dispatcher.BeginInvoke(new Action(delegate
//            {
//                var filePath = fileControl.Text;
//                var changedFilePath = e.FullPath;
//                if (changedFilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase))
//                    publishControl.Background = Brushes.LightCoral;
//            }));
//        }


//        private static Guid _resource1Id;
//        private static Guid _resource2Id;
//        private static Guid _resource3Id;
//        private static Guid _resource4Id;
//        private static Guid _resource5Id;
//        private static Guid _resource6Id;
//        private static Guid _resource7Id;
//        private static Guid _resource8Id;
//        private static Guid _resource9Id;
//        private static Guid _resource10Id;
//        private static Guid _resource11Id;
//        private static Guid _resource12Id;


//        private void Publish1_Click(object sender, RoutedEventArgs e)
//        {
//            _resource1Id = Publish(File1, Resource1, Publish1, _resource1Id);
//        }


//        private void Publish2_Click(object sender, RoutedEventArgs e)
//        {
//            _resource2Id = Publish(File2, Resource2, Publish2, _resource2Id);
//        }


//        private void Publish3_Click(object sender, RoutedEventArgs e)
//        {
//            _resource3Id = Publish(File3, Resource3, Publish3, _resource3Id);
//        }


//        private void Publish4_Click(object sender, RoutedEventArgs e)
//        {
//            _resource4Id = Publish(File4, Resource4, Publish4, _resource4Id);
//        }


//        private void Publish5_Click(object sender, RoutedEventArgs e)
//        {
//            _resource5Id = Publish(File5, Resource5, Publish5, _resource5Id);
//        }


//        private void Publish6_Click(object sender, RoutedEventArgs e)
//        {
//            _resource6Id = Publish(File6, Resource6, Publish6, _resource6Id);
//        }


//        private void Publish7_Click(object sender, RoutedEventArgs e)
//        {
//            _resource7Id = Publish(File7, Resource7, Publish7, _resource7Id);
//        }


//        private void Publish8_Click(object sender, RoutedEventArgs e)
//        {
//            _resource8Id = Publish(File8, Resource8, Publish8, _resource8Id);
//        }


//        private void Publish9_Click(object sender, RoutedEventArgs e)
//        {
//            _resource9Id = Publish(File9, Resource9, Publish9, _resource9Id);
//        }


//        private void Publish10_Click(object sender, RoutedEventArgs e)
//        {
//            _resource10Id = Publish(File10, Resource10, Publish10, _resource10Id);
//        }


//        private void Publish11_Click(object sender, RoutedEventArgs e)
//        {
//            _resource11Id = Publish(File11, Resource11, Publish11, _resource11Id);
//        }
//        private void Publish12_Click(object sender, RoutedEventArgs e)
//        {
//            _resource12Id = Publish(File12, Resource12, Publish12, _resource12Id);
//        }


//        private Guid Publish(TextBox fileControl, TextBox resourceControl, Button publishControl, Guid resourceId)
//        {
//            var filePath = fileControl.Text;
//            var resourceName = resourceControl.Text;
//            if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(resourceName))
//                return resourceId;

//            publishControl.Content = "Process...";

//            var thread = new Thread(() =>
//            {
//                if (resourceId == default(Guid))
//                {
//                    resourceId = App.Service.RetrieveMultiple(new FetchExpression($@"
//<fetch top='1' no-lock='true' >
//  <entity name='webresource' >
//    <attribute name='name' />
//    <attribute name='webresourceid' />
//    <filter>
//      <condition attribute='name' operator='eq' value='{resourceName}' />
//    </filter>
//  </entity>
//</fetch>")).Entities.First().Id;
//                }
                
//                App.Service.Update(new Entity("webresource", resourceId) { ["content"] = Convert.ToBase64String(File.ReadAllBytes(filePath)) });
//                App.Service.Execute(new OrganizationRequest
//                {
//                    RequestName = "PublishXml",
//                    Parameters = new ParameterCollection { new KeyValuePair<string, object>("ParameterXml", $"<importexportxml><webresources><webresource>{resourceId}</webresource></webresources></importexportxml>") }
//                });

//                Dispatcher.BeginInvoke(new Action(delegate
//                {
//                    publishControl.Content = "Publish";
//                    publishControl.Background = SystemColors.ControlBrush;
//                }));
//            });
//            thread.Start();
//            return resourceId;
//        }


//        private void OpenBuildJsWindow(object sender, RoutedEventArgs e)
//        {
//            new BuildJSWindow().Show();
//        }


//        private void OpenWebResourcesWindow(object sender, RoutedEventArgs e)
//        {
//            new WebResourcesWindow().Show();
//        }

//        private void OpenWorkflowLogAnalizerWindow(object sender, RoutedEventArgs e)
//        {
//            new WorkflowLogAnalizerWindow().Show();
//        }
    }
}