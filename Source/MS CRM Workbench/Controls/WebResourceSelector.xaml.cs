using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PZone.Models;
using PZone.ViewModels;
using WebResource = PZone.Models.WebResource;


namespace PZone.Controls
{
    /// <summary>
    /// Interaction logic for WebResourceSelector.xaml
    /// </summary>
    public partial class WebResourceSelector : UserControl
    {
        private static readonly DependencyProperty _selectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(WebResource), typeof(WebResourceSelector), new PropertyMetadata(null));
        

        public WebResourceSelector()
        {
            InitializeComponent();
            GetResources();
        }


        private void GetResources()
        {
            var resources = App.Service.RetrieveMultiple(new FetchExpression(
                "<fetch no-lock=\"true\"><entity name=\"webresource\">" +
                "<attribute name=\"name\" /><attribute name=\"displayname\" /><attribute name=\"webresourcetype\" /><attribute name=\"description\" />" +
                "<filter><condition attribute=\"iscustomizable\" operator=\"eq\" value=\"true\" /><condition attribute=\"ismanaged\" operator=\"eq\" value=\"0\" /></filter>" +
                "</entity></fetch>"));
            foreach (var sourceResource in resources.Entities)
            {
                var resource = new WebResource(sourceResource);
                var pathParts = resource.Name.Split('/').ToList();
                pathParts.RemoveAt(pathParts.Count - 1);
                var treeLevel = ResourcesTree;
                var rootLevel = true;
                foreach (var pathPart in pathParts)
                {
                    var level = (WebResourceFolder)treeLevel.FirstOrDefault(l => (l as WebResourceFolder)?.Name == pathPart);
                    if (level == null)
                    {
                        level =  new WebResourceFolder(pathPart);
                        if (rootLevel)
                            level.Type = WebResourceFolderType.Root;
                        treeLevel.Add(level);
                    }
                    treeLevel = level.Items;
                    rootLevel = false;
                }
                treeLevel.Add(resource);
            }
        }

        
        private void OpenPopup(object sender, RoutedEventArgs e)
        {
            PopupControl.IsOpen = true;
        }


        private void SelectItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = ((TreeView)sender).SelectedItem;
            if(selectedItem == null || selectedItem is WebResourceFolder)
                return;
            var trvItem = (WebResource)selectedItem;
            ValueField.Text = trvItem.Name;
            SelectedItem = trvItem;
            PopupControl.IsOpen = false;
        }


        private void DisableDoubleClickFolderSelection(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = GetClickedItem(e);
            if (!(clickedItem?.DataContext is WebResourceFolder))
                return;
            clickedItem.IsExpanded = !clickedItem.IsExpanded;
            clickedItem.IsSelected = false;
            e.Handled = true;
        }
        

        private static TreeViewItem GetClickedItem(RoutedEventArgs e)
        {
            var hit = e.OriginalSource as DependencyObject;
            while (hit != null && !(hit is TreeViewItem))
                hit = VisualTreeHelper.GetParent(hit);
            return hit as TreeViewItem;
        }


        public WebResource SelectedItem
        {
            get { return GetValue(_selectedItemProperty) as WebResource; }
            set { SetValue(_selectedItemProperty, value); }
        }


        public ObservableCollection<ObservableObject> ResourcesTree { get; set; } = new ObservableCollection<ObservableObject>();


        private void DisableClickFolderSelection(object sender, RoutedEventArgs e)
        {
            var clickedItem = GetClickedItem(e);
            if (!(clickedItem?.DataContext is WebResourceFolder))
                return;
            clickedItem.Focusable = false;
            e.Handled = true;
        }
    }
}
