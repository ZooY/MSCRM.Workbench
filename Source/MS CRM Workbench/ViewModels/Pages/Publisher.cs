using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PZone.Models;
using PZone.Wpf;


namespace PZone.ViewModels.Pages
{
    public class Publisher : ViewModel
    {
        private static readonly object _lock = new object();

        private ICommand _addWebResourceCommand;
        private static readonly FileSystemWatcher _watcher = new FileSystemWatcher
        {
            Path = @"C:\Projects\CRM\CRM\Source",
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        public ObservableCollection<WebResource> WebResources { get; set; } = new ObservableCollection<WebResource>();


        public Publisher()
        {
            _watcher.Changed += CheckChanged;
            _watcher.Created += CheckChanged;
            _watcher.Renamed += CheckChanged;

            WebResources.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (WebResource item in e.OldItems)
                    {
                        _watcher.Changed -= item.CheckChanged;
                        _watcher.Created -= item.CheckChanged;
                        _watcher.Renamed -= item.CheckChanged;
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (WebResource item in e.NewItems)
                    {
                        _watcher.Changed += item.CheckChanged;
                        _watcher.Created += item.CheckChanged;
                        _watcher.Renamed += item.CheckChanged;
                    }
                }
            };
        }


        public ICommand AddWebResource
        {
            get { return _addWebResourceCommand ?? (_addWebResourceCommand = new Command(o => { WebResources.Add(new WebResource()); })); }
        }


        private void CheckChanged(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath.ToLower();
            if (!filePath.EndsWith(".js") && !filePath.EndsWith(".css") && !filePath.EndsWith(".html") && !filePath.EndsWith(".htm"))
                return;
            var relativePath = filePath.Substring(_watcher.Path.Length);
            if (relativePath.Contains("\\_"))
                return;
            var fileName = Path.GetFileName(filePath);
            //if (fileName.StartsWith("_"))
            //    return;
            if (fileName.Equals("app_offline.htm"))
                return;
            
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate
            {
                lock (_lock)
                {
                    if (WebResources.Any(wr => wr.FilePath == filePath))
                        return;
                    var resource = new WebResource(null, filePath);
                    _watcher.Changed += resource.CheckChanged;
                    _watcher.Created += resource.CheckChanged;
                    _watcher.Renamed += resource.CheckChanged;
                    resource.CheckChanged(null, e);
                    WebResources.Add(resource);
                }
            }));
        }
    }
}